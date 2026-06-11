# Docker Containerization — Design Spec

**Date:** 2026-06-11
**Status:** Approved design, pending implementation
**Roadmap phase:** Month 1 (Docker), precedes Month 2 (Azure deployment)

## Goal

Containerize the MarketerSysytem API so that:

1. `docker compose up` from a fresh clone gives a fully working API + SQL Server, with migrations and seed data applied — no LocalDB, no Visual Studio required.
2. The **same image** built here ships unchanged to Azure App Service in Month 2. Compose is one consumer of the image, not its definition.
3. The Dockerfile demonstrates senior-level container practice for portfolio reviewers: multi-stage build, layer-cached restore, minimal non-root runtime image.

## Decision

**Production-shaped multi-stage Dockerfile on a chiseled Ubuntu runtime, plus a dev-only docker-compose.**

Alternatives considered and rejected:

- **Full Debian `aspnet:10.0` runtime** — easier to `docker exec` into (has a shell), but weaker security posture and weaker portfolio signal. Chiseled was chosen; if in-container debugging is ever needed, a temporary swap of the final base image is a one-line change.
- **.NET Aspire AppHost** — modern orchestration, but it layers an abstraction that the Month 2 Azure App Service deployment doesn't need. Revisit when the Month 4 microservice split makes multi-service orchestration real.

## Topology

Two services in `docker-compose.yml` (repo root):

| Service | Image | Role |
|---|---|---|
| `db` | `mcr.microsoft.com/mssql/server:2022-latest` | SQL Server 2022 Developer edition, named volume `mssql_data` for persistence |
| `web` | built from `./Dockerfile` | The ASP.NET Core API |

- `web` starts only after `db` reports healthy (`depends_on: condition: service_healthy`).
- `web` reaches the database at hostname `db`, port `1433` (compose network DNS).
- Host port mapping: `8080:8080` for the API, `1433:1433` for the database (so SSMS/Azure Data Studio on the host can inspect it).

## Dockerfile (repo root)

Two named stages, three logical phases:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Phase 1 — restore (cached unless project files / central props change)
COPY Directory.Build.props Directory.Packages.props ./
COPY MarketerSystem.Domain/MarketerSystem.Domain.csproj MarketerSystem.Domain/
COPY MarketerSystem.Common/MarketerSystem.Common.csproj MarketerSystem.Common/
COPY MarketerSystem.Abstractions/MarketerSystem.Abstractions.csproj MarketerSystem.Abstractions/
COPY MarketerSystem.Data/MarketerSystem.Data.csproj MarketerSystem.Data/
COPY MarketerSystem.Repository/MarketerSystem.Repository.csproj MarketerSystem.Repository/
COPY MarketerSystem.Service/MarketerSystem.Service.csproj MarketerSystem.Service/
COPY MarketerSysytem.Web/MarketerSysytem.Web.csproj MarketerSysytem.Web/
RUN dotnet restore MarketerSysytem.Web/MarketerSysytem.Web.csproj

# Phase 2 — publish
COPY . .
RUN dotnet publish MarketerSysytem.Web/MarketerSysytem.Web.csproj \
    -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Phase 3 — minimal runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MarketerSysytem.Web.dll"]
```

Key constraints and rationale:

- **Build context is the repo root.** Restore needs `Directory.Build.props` + `Directory.Packages.props` (central version management) plus the project files. Only the 7 csprojs in the Web dependency graph are copied for restore; `MarketerSystem.Tests` is not part of the published graph.
- **Restore is its own cached layer.** Source-only changes skip the NuGet restore entirely; only csproj/props edits invalidate it.
- **Chiseled runtime** (`aspnet:10.0-noble-chiseled`): distroless-style Ubuntu, no shell, no package manager, runs as the non-root `app` user by default (no `USER` directive needed). Final image ≈ 120 MB.
- **`/p:UseAppHost=false`** — entrypoint is `dotnet MarketerSysytem.Web.dll`, so the native apphost executable is dead weight.
- **Assembly name stays `MarketerSysytem.Web.dll`** — the project-name typo is load-bearing (see CLAUDE.md); do not "fix" it in the Dockerfile.
- **Port:** the `aspnet:10.0` image family defaults to `ASPNETCORE_HTTP_PORTS=8080`; `EXPOSE 8080` documents it. HTTP only — TLS terminates at the platform edge (see Code change below).

## .dockerignore (repo root)

Keeps the build context small and prevents host build artifacts leaking into `COPY . .`:

```
**/bin/
**/obj/
**/TestResults/
.vs/
.git/
.gitignore
docs/
*.user
*.md
CLAUDE*.md
**/CLAUDE*.md
.env
.env.example
docker-compose.yml
```

(`*.md` already covers CLAUDE files, but the explicit entries guard against the pattern being loosened later — these files must never enter an image.)

## docker-compose.yml (repo root, dev only)

```yaml
services:
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: "Y"
      MSSQL_PID: Developer
      MSSQL_SA_PASSWORD: ${DB_PASSWORD}
    ports:
      - "1433:1433"
    volumes:
      - mssql_data:/var/opt/mssql
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P \"$$MSSQL_SA_PASSWORD\" -Q \"SELECT 1\" -C || exit 1"]
      interval: 10s
      timeout: 5s
      retries: 10
      start_period: 30s

  web:
    build: .
    depends_on:
      db:
        condition: service_healthy
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__MarketerDBContext: "Server=db,1433;Database=MarketerDB;User Id=sa;Password=${DB_PASSWORD};TrustServerCertificate=True;MultipleActiveResultSets=True"
    ports:
      - "8080:8080"

volumes:
  mssql_data:
```

Notes:

- **`ASPNETCORE_ENVIRONMENT=Development`** is deliberate: it keeps the existing dev-gated startup behavior — `Database.Migrate()` (schema + seed data), `/openapi/v1.json`, and `/scalar/v1` all active. This compose file is a development environment; production environment shape arrives with Azure in Month 2.
- **`ConnectionStrings__MarketerDBContext`** (double underscore = config hierarchy separator) overrides the LocalDB string in `appsettings.json` via the standard environment-variable configuration provider. **No code change needed for configuration.** Non-Docker dev (VS F5 against LocalDB) keeps working untouched.
- **Healthcheck** uses `sqlcmd` from `mssql-tools18` (bundled in the SQL 2022 image). `-C` trusts the container's self-signed certificate — mandatory-encryption default in tools18 fails the connection otherwise. `$$` escapes the `$` so the variable resolves inside the container at runtime, not at compose-file interpolation time.
- **`MSSQL_SA_PASSWORD`** is the current variable name (`SA_PASSWORD` is deprecated).

## Secrets

- **`.env`** at repo root holds `DB_PASSWORD=<dev-only password>`. Added to `.gitignore`. Compose reads it automatically.
- **`.env.example`** is checked in with a placeholder and a comment stating SQL Server's complexity rule (min 8 chars, 3 of 4 character classes) — a too-weak password makes the `db` container exit immediately, which is the most common first-run failure.
- `appsettings.json` keeps the LocalDB connection string for non-Docker dev. No secrets live in tracked files.

## Migrations & seed data

Unchanged: `Program.cs` runs `context.Database.Migrate()` at startup when the environment is `Development`. Compose sets exactly that environment, so a fresh `docker compose up` creates `MarketerDB`, applies all migrations, and inserts seed data (2 distributors, 4 products, 1 sell, 1 bonus payment).

Production migration strategy (separate init step/pipeline, not startup-time) is **explicitly deferred to Month 2** — documented here so it isn't mistaken for an oversight.

## Healthchecks

- **`db`:** compose-level `sqlcmd` probe (above). Gates `web` startup.
- **`web`:** no container-level `HEALTHCHECK` — the chiseled image has no shell/curl/wget to run one. The app's `/healthz` endpoint (EF Core DbContext probe, already implemented) is the health surface; Azure App Service / orchestrators consume it over HTTP in later months. Verification (below) exercises it manually.

## Code change (the only one)

Remove `app.UseHttpsRedirection();` from `MarketerSysytem.Web/Program.cs`.

The container binds HTTP 8080 only; with no HTTPS port configured the middleware logs a "Failed to determine the https port for redirect" warning on every request and does nothing useful. TLS termination is the platform's job (App Service front end, ingress, reverse proxy) — removing the middleware is the cloud-native convention, not a security regression.

## Documentation deliverables

1. **`README.md`** (currently a one-line stub) — add a "Run with Docker" section: prerequisites (Docker Desktop with WSL2 backend on Windows), `cp .env.example .env` + set password, `docker compose up --build`, endpoints (`http://localhost:8080/scalar/v1`, `/healthz`, `/api/...`), connecting SSMS to `localhost,1433`, and troubleshooting (SA password complexity, port 1433/8080 conflicts, first-pull time).
2. **`CLAUDE.md`** — update Build/Run/Test section with the Docker run path and the new file inventory. (Local-only file; per standing rule it stays gitignored and current.)

## File inventory (implementation checklist)

| File | Action |
|---|---|
| `Dockerfile` | create (repo root) |
| `.dockerignore` | create (repo root) |
| `docker-compose.yml` | create (repo root) |
| `.env.example` | create (repo root) |
| `.env` | create locally, never committed |
| `.gitignore` | append `.env` |
| `MarketerSysytem.Web/Program.cs` | remove `app.UseHttpsRedirection();` |
| `README.md` | add "Run with Docker" section |
| `CLAUDE.md` | update (untracked) |

## Verification

1. `docker compose build` completes cleanly; `docker image ls` shows the web image ≈ 250 MB or less.
2. `docker compose up`: `db` reaches healthy; `web` starts; logs show EF migrations applied.
3. `GET http://localhost:8080/healthz` → 200 `Healthy`.
4. `http://localhost:8080/scalar/v1` renders the API reference.
5. `GET http://localhost:8080/api/Distributor` returns the 2 seeded distributors (proves DB connectivity + migration + seed end-to-end).
6. Regression: `docker compose down -v` (wipe volume) then `up` again — fresh database migrates and seeds correctly.
7. Non-Docker path still works: `dotnet run --project MarketerSysytem.Web` against LocalDB, and `dotnet test` still 21/21.

## Out of scope (deferred)

- Production hardening of the SQL Server container — it is dev-only; Month 2 replaces it with Azure SQL.
- CI image build/publish (GitHub Actions → registry) — Month 2.
- Startup-time migration removal / dedicated migration runner — Month 2.
- Multi-arch builds, BuildKit cache mounts, and a `test` stage in the Dockerfile — add only if build times or CI needs justify them.
