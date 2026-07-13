# Docker Knowledge — MarketerSystem

> Knowledge file — committed to the repo (user request 2026-07-13, learning purposes). Updated: 2026-07-13.
> Status: **design committed, implementation not started.** The public spec lives at
> `docs/superpowers/specs/2026-06-11-docker-design.md` (committed, portfolio-visible) — that file is the
> contract; this file is working knowledge around it.

## Design decisions (from the approved-pending-review spec)

- **Multi-stage Dockerfile at repo root**, build context = repo root (needed for `Directory.Build.props` + `Directory.Packages.props`).
- Stage 1 `sdk:10.0`: copy the 2 props files + the **7 csprojs in the Web dependency graph** (everything except Tests), `dotnet restore MarketerSysytem.Web/MarketerSysytem.Web.csproj` → cached restore layer; then `COPY . .` + `dotnet publish -c Release /p:UseAppHost=false --no-restore`.
- Stage 2 **`aspnet:10.0-noble-chiseled`**: distroless-style, non-root `app` user by default, no shell, ≈120 MB. `ENTRYPOINT ["dotnet", "MarketerSysytem.Web.dll"]` — typo'd assembly name is load-bearing.
- **docker-compose.yml is dev-only** (the image is the artifact; compose is one consumer): `db` = `mcr.microsoft.com/mssql/server:2022-latest` with named volume + `sqlcmd` healthcheck (`/opt/mssql-tools18/bin/sqlcmd ... -C`, `$$` escaping, `MSSQL_SA_PASSWORD` not the deprecated `SA_PASSWORD`); `web` waits on `condition: service_healthy`.
- **Config via env vars:** `ConnectionStrings__MarketerDBContext=Server=db,1433;...` overrides appsettings LocalDB string — no code change. `ASPNETCORE_ENVIRONMENT=Development` in compose keeps dev-gated `Migrate()` + Scalar active.
- **Secrets:** `.env` (gitignored, holds `DB_PASSWORD`) + committed `.env.example`. SQL Server password complexity (8+ chars, 3 of 4 classes) or the db container exits immediately — most common first-run failure.
- **One code change:** remove `app.UseHttpsRedirection()` (TLS terminates at platform edge). Note: it currently sits *before* `UseRouting()` after the 2026-06-11 ordering fix; removal supersedes that.
- `.dockerignore` must exclude `**/bin`, `**/obj`, `.git/`, `docs/`, `*.md`, and **all `CLAUDE*` files** (never in an image).
- Port 8080 (image default `ASPNETCORE_HTTP_PORTS=8080` on .NET 8+ aspnet images); no container HEALTHCHECK for web (chiseled has no curl) — `/healthz` is the health surface for platforms.

## Container-critical gotchas learned 2026-06-11 (the hard way)

1. **EF 10 `Migrate()` validates pending model changes at startup and THROWS.** Non-deterministic seed values (`DateTime.Now`, `Guid.NewGuid()`) made startup fail everywhere. Fixed via deterministic constants + migration `20260611_DeterministicSeedAndMoneyPrecision`.
2. **Timezone sensitivity = container killer.** Implicit `DateTime` → `DateTimeOffset` conversion in seed data bakes the machine's local offset (+04:00 here) into the model. **Containers run UTC** → model ≠ snapshot → startup crash *only inside Docker*. Fixed with explicit `TimeSpan.Zero` offsets; if a future seed/model change reintroduces an implicit conversion, the app will boot on this Windows machine and die in the container. Check the snapshot for `TimeSpan(0, 4, ...)` residue after any migration.
3. Migration tooling: `dotnet ef migrations add <Name> --project MarketerSystem.Data --startup-project MarketerSysytem.Web` (dotnet-ef 10.0.9 installed globally).

## Verification path (from the spec)

`docker compose build` (image ≤ ~250 MB) → `up` → db healthy → web migrates+seeds → `/healthz` 200 → `/scalar/v1` renders → `GET /api/Distributor` returns 2 seeded distributors → `down -v` + `up` re-seeds cleanly → non-Docker `dotnet run` + `dotnet test` (55) still green.

Equivalent non-Docker smoke test already proven 2026-06-11: healthz 200, Distributor/BonusPayment/Sell endpoints 200 on LocalDB at `http://localhost:5023`.

## Forward links

- **Month 2 (Azure):** same image to App Service; migrations move out of startup into a deploy-time step; SQL container → Azure SQL; `.env` → Key Vault/App Config.
- **Month 4 (microservices, see CLAUDE-microservices.md):** compose grows 2 → N containers (PayoutService + broker). Keep the Dockerfile pattern copy-pasteable: per-service Dockerfile, shared props copy block.
- Windows host note: Docker Desktop with WSL2 backend required (Windows 10 Pro 19045 here).
