# MarketerSysytem

ASP.NET Core Web API tracking **distributors**, **product sales**, and **multi-level marketing bonus payouts**. Used as a portfolio piece to demonstrate senior .NET skills — currently being modernized through a 6-month roadmap (Docker → Azure → caching/messaging → microservices/observability).

**Supplementary knowledge files** (committed to the repo — user decision 2026-07-13, learning-in-public): `CLAUDE-docker.md` (containerization design + container-critical gotchas), `CLAUDE-microservices.md` (decomposition analysis + agreed Month-4 direction: grow functionality, then staged split starting with the payout engine), `CLAUDE-docker-learning.md` (the user's hands-on Docker learning guide — rewritten 2026-07-13 as a **self-study** document: the user works through it in their own terminal to save tokens; expected outputs and quiz answers are inline. If they say "docker learning", resume at the first unchecked item; still never write Docker files for them — they draft, Claude reviews).

**Standing rule (user request 2026-07-13):** the user learns across many separate sessions — update `CLAUDE-docker-learning.md` (tick checkboxes + append session-log rows) **immediately as progress happens**, without being asked, so any future session can resume from the file alone. Keep all knowledge files current whenever facts change.

## Stack

- **.NET 10 LTS** (SDK 10.0.300, runtime 10.0.8), `dotnet-ef` global tool 10.0.9 installed
- **ASP.NET Core** Web API with controllers, `ControllerBase`, primary constructors throughout
- **EF Core 10.0.8** on SQL Server (LocalDB in dev; `(localdb)\mssqllocaldb`, db `MarketerDB`)
- **Mapster 10.0.7** for object mapping (`IRegister` profiles, `MapsterMapper.IMapper`)
- **Microsoft.Identity.Web 4.10** for Azure AD JWT — wired but placeholder config; no `[Authorize]` on controllers yet
- **`Microsoft.AspNetCore.OpenApi` + Scalar.AspNetCore 2.14** — native OpenAPI + Scalar UI in dev (`launchUrl` is `scalar/v1`)
- **Health checks** with EF Core DbContext probe at `/healthz`
- **Global exception handler** + `ProblemDetails` for RFC 7807 error responses
- **Tests:** xUnit v3 (3.2.2) + Moq 4.20 + Shouldly 4.3 + EF Core InMemory — **55 tests passing**

AutoMapper and FluentAssertions were removed during the upgrade (both went commercial). Do **not** suggest reintroducing them.

## Centralized configuration

`Directory.Packages.props` + `Directory.Build.props` at repo root own all common settings:

- **`Directory.Build.props`** — `<TargetFramework>net10.0</TargetFramework>`, `<Nullable>enable</Nullable>`, `<ImplicitUsings>enable</ImplicitUsings>`, `<LangVersion>latest</LangVersion>`
- **`Directory.Packages.props`** — `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>` + all `<PackageVersion>` entries

Individual csprojs hold only project-specific concerns. `MarketerSystem.Service` additionally references `Microsoft.EntityFrameworkCore.Relational` (needed for `AsSplitQuery`).

## Solution Layout

8 projects, Clean Architecture stack. Dependency direction (rightmost is innermost):

```
Web ──> Service ──> Abstractions      Web ──> Repository ──> Data ──> Domain
        Service ──> Common/Domain      Repository ──> Abstractions
```

Note: Service does **not** reference Repository/Data — it depends only on the repository **interfaces** in Abstractions. Web wires implementations via DI.

| Project | Role |
|---|---|
| `MarketerSystem.Domain` | POCO entities (`Distributor`, `Sell`, `BonusPayment`, `Product`, `Address`, `Passport`, `Picture`, `ContactInfo`), `ResourceParameters` filter classes, `Policies/BonusPercentages`, `Policies/GenerationChain` |
| `MarketerSystem.Common` | DTOs (`XDTO` read / `XCreateDTO` write) and enums |
| `MarketerSystem.Abstractions` | `IRepositoryBase<T>`, `IUnitOfWork`, `IServiceBase<T>`, plus per-entity `IXRepository`/`IXService` (mostly one-line markers; `IDistributorService`, `ISellService`, `IBonusPaymentService` have real members) |
| `MarketerSystem.Data` | `MarketerDBContext` (also implements `IUnitOfWork`), EF migrations, deterministic seed data |
| `MarketerSystem.Repository` | `RepositoryBase<T>` + per-entity repos (primary-constructor one-liners) |
| `MarketerSystem.Service` | `ServiceBase<TEntity, TRepository>` + per-entity services; business logic lives here (`BonusPaymentService`, `DistributorService`, `SellService`) |
| `MarketerSysytem.Web` | Thin controllers (validate/map/delegate), Mapster `IRegister` profiles in `Profiles/`, `Program.cs`, `Infrastructure/GlobalExceptionHandler.cs` |
| `MarketerSystem.Tests` | xUnit v3 tests (55 passing) |

**Typo warning:** the solution file, web project folder, and web project name spell it `MarketerSysytem` (extra `y`). The other 7 projects use the correct `MarketerSystem`. Both spellings are load-bearing — don't "fix" them without explicit user OK.

## Key Patterns

**IQueryable repository.** `IRepositoryBase<T>.Set()` returns `IQueryable<T>` — the composable query root. Services compose `Where`/`Include`/`AsSplitQuery` on it and materialize with `ToListAsync()`. Filters translate to SQL; nothing loads a full table. **Controllers never see IQueryable** — they call intention-revealing service methods (`ListAsync`, `ListWithDetailsAsync`, `FilterSoldProducts`, …) that return materialized lists.

**Unit of Work.** `MarketerDBContext` implements `IUnitOfWork` and is DI-registered as both (same scoped instance). `RepositoryBase` takes `IUnitOfWork` and casts to `MarketerDBContext` in the constructor — throws `ArgumentException` if the cast fails.

**Repository `SaveAsync` = add-if-detached.** Tracked entities persist their modifications on commit without re-adding; `SaveAsync` only `Add`s detached (new) entities. There is no separate `AddAsync` on the contract anymore.

**Mutations commit immediately at the service layer** (`ServiceBase.SaveAsync`/`DeleteAsync` call `CommitAsync`), **except** multi-step service methods (`GenerateBonusPaymentsForPeriodAsync`, `DistributorService.UpdateAsync`) which use repo-level saves and one `CommitAsync` at the end — atomic.

**Business logic belongs to services, not controllers.**
- `DistributorService.CreateAsync` — assigns the Guid, validates the recommender exists (400 if not), enforces `GenerationChain.MaxDepth`, builds the linker.
- `DistributorService.UpdateAsync` — updates scalars and **replaces** any provided child collections (PUT is idempotent; repeated PUT no longer duplicates children); null/empty child collections leave existing children untouched; single commit.
- `DistributorService.ListWithDetailsAsync`/`FetchWithDetailsAsync` — `Include` × 4 + `AsSplitQuery` (no N+1).
- `SellService.CreateSellAsync` — validates product/distributor exist (throws `ArgumentException` → 400 ProblemDetails), snapshots all three price fields from `Product.Price`, sets navs for the response DTO.
- `BonusPaymentService.GenerateBonusPaymentsForPeriodAsync` — see Domain Highlights; returns the created payments (POST responds with them).

**Exception → status mapping.** Services throw (`ArgumentNullException`/`ArgumentException` → 400, `InvalidOperationException` → 409, `KeyNotFoundException` → 404) and `GlobalExceptionHandler` renders RFC 7807 ProblemDetails. Controllers return `NotFound()` for missing aggregates; validation errors flow as exceptions.

**Mapping via Mapster `IRegister`.** Profiles in `MarketerSysytem.Web/Profiles/*.cs`. Mapster also maps nested DTOs (e.g. `DistributorCreateDTO.Addresses` → entity children) **by convention** — explicit configs exist for documentation/customization points. EF persists mapped child graphs on `Add` (navigation cascade).

**File-scoped namespaces and primary constructors** are the house style.

## Domain Highlights

- **`Distributor`** is the root entity. 1:1 → `Passport`, 1:N → `Address`, `ContactInfo`, `Picture`, `BonusPayment`. Self-referencing via `RecomendatorID`.
- **`GenerationChain` (`MarketerSystem.Domain.Policies`)** — owns the `GenerationLinker` format (comma-separated distributor IDs, root-first / immediate-recommender-last). `Parse` (skips invalid entries), `Append`, `Depth`, `IsAtMaxDepth` (≥ 5 blocks). All encode/decode goes through it — no inline `Split(',')` anywhere else.
- **`BonusPercentages`** — `DirectSeller = 0.10m`, `Recommender = 0.05m`, `GrandRecommender = 0.01m`, `ForUplineLevel(level)` returns 0 beyond level 1.
- **`BonusPaymentService.GenerateBonusPaymentsForPeriodAsync`** — fetches unpaid sales in the window (SQL-side filter), batch-fetches sellers (one `Contains` query, no N+1), flags `UsedForPayment` (tracked mutation), pays seller 10% + upline by level, **one atomic commit**, returns created payments. A sale whose seller row is missing still pays the direct seller; only the upline walk is skipped.
- **Seed data is deterministic** (fixed GUIDs, fixed `DateTimeOffset`s with explicit zero offset). Critical: EF 10's `Migrate()` validates pending model changes at startup — non-deterministic seeds (`DateTime.Now`, `Guid.NewGuid()`) or implicit `DateTime`→`DateTimeOffset` conversions (which bake in the machine's local offset) make startup fail. Money columns have explicit `HasPrecision(18, 2)`.
- **Migrations:** 2024-era initial + seed, plus `20260611_DeterministicSeedAndMoneyPrecision` (UpdateData only, no schema ops). Applied at startup via `context.Database.Migrate()` — **dev only**.

## Build, Run, Test

```powershell
dotnet restore
dotnet build                                                    # 0 warnings, 0 errors
dotnet test MarketerSystem.Tests/MarketerSystem.Tests.csproj    # 55 tests
dotnet run --project MarketerSysytem.Web                        # auto-migrates LocalDB in dev
# migrations:
dotnet ef migrations add <Name> --project MarketerSystem.Data --startup-project MarketerSysytem.Web
```

**Dev endpoints:** `/openapi/v1.json`, `/scalar/v1`, `/healthz`. Verified working smoke path: healthz 200, `GET /api/Distributor` returns seeded distributors with passports, filtered `GET /api/BonusPayment?MinPrice=5` 200.

**.NET 10 SDK requirement.** VS 2022 before ~17.13 cannot load the .NET 10 SDK — it falls back to an older SDK and fails with NETSDK1045 ("current .NET SDK does not support targeting .NET 10.0"). **Confirmed on this machine (2026-06-12):** SDKs installed are 5.0.416 / 8.0.101 / 10.0.300; VS 2022 Community is **17.8** (too old → VS builds fail until updated via Visual Studio Installer) and a VS 2019 Enterprise also exists (irrelevant). CLI builds always work. No `global.json` in the repo.

## Cross-cutting

- **`GlobalExceptionHandler`** maps exception types → ProblemDetails (400/404/409/500), logs with request method/path.
- **Health checks** at `/healthz` with `DbContextCheck<MarketerDBContext>`.
- **Middleware order:** ExceptionHandler → StatusCodePages → (dev: OpenAPI/Scalar/migrate) → HttpsRedirection → Routing → AuthN → AuthZ → MapControllers → MapHealthChecks. (`UseHttpsRedirection` is slated for removal in the Docker phase — TLS terminates at the platform edge.)

## API behavior notes (changed 2026-06-11)

- `PUT /api/Distributor/{id}` **replaces** provided child collections (was: append → duplicates).
- `POST /api/BonusPayment` returns the created payments as DTOs (was: bare 201).
- Validation failures return ProblemDetails JSON (was: ad-hoc `{error}` objects).
- `GET /api/Sell/{id}` is typed `ActionResult<SellDTO>` (was wrongly `ProductDTO` in the OpenAPI schema).

## Known Issues (intentionally not yet fixed)

1. **`DocumentType.Pasport`** enum typo — renaming requires migration + DTO/API surface decisions.
2. **Project name typo `MarketerSysytem`** kept by user choice.
3. **No `[Authorize]` attributes** — auth pipeline registered with placeholder Azure AD config; wired when real Entra ID values exist.
4. **`Sell` has three price columns** (`ProductPrice`/`ProductUnitPrice`/`ProductTotalPrice`) that always carry the same value (no quantity column). Collapsing them needs a migration + DTO change; `SellService.CreateSellAsync` is the single place that sets them.
5. **`GenerationLinker` is `MaxLength(50)`** — five large IDs can approach the limit; revisit if real ID ranges grow.

## Testing

`MarketerSystem.Tests/Infrastructure/InMemoryContextFactory.cs` creates a fresh `MarketerDBContext` on EF Core InMemory (unique DB name per test). InMemory does **not** apply `HasData` seeds (no `EnsureCreated` call) — tests seed explicitly.

**Two-tier strategy:** service behavior tests run on **EF InMemory with real repositories** (queries, tracking, commits execute for real — mocks can't fake `IQueryable` translation, and `ToListAsync` throws on mocked lists); `ServiceBase` orchestration contract (repo call + commit) is pinned with **Moq** in `ProductServiceTests`. xUnit v3 note: direct `SaveChangesAsync()` calls in test bodies need `TestContext.Current.CancellationToken` (analyzer xUnit1051).

Current coverage (55 tests):
- `Services/BonusPaymentServiceTests` — filters (InMemory + Include), MLM generation (direct %, upline levels, idempotent skip, window filter, missing-seller tolerance) (13)
- `Services/DistributorServiceTests` — chain building (root/chained/missing/depth-limit), update replace-children idempotency, details queries (13)
- `Services/SellServiceTests` — create validation + price snapshot, filters (7)
- `Services/ProductServiceTests` — ServiceBase contract via mocks + ListAsync (6)
- `Domain/GenerationChainTests` — parse/append/depth/max-depth (12 cases)
- `Repositories/BonusPaymentRepositoryTests` — EF InMemory round-trip (4)

**Not yet covered:** controllers (thin — low priority), Mapster profile shape assertions.

## Roadmap Context

Upgrade + refactor pass complete (2026-06-09); deep-review optimization pass complete (2026-06-11): IQueryable repositories, service-layer business logic, GenerationChain policy, deterministic seeds + money precision migration, 55 tests.

Next phases:
1. **Month 1** — Docker (spec approved-pending-review at `docs/superpowers/specs/2026-06-11-docker-design.md`)
2. **Month 2** — Azure deployment (App Service, SQL DB, Key Vault, App Insights)
3. **Month 3** — Redis caching, RabbitMQ/Service Bus, Outbox pattern
4. **Month 4+** — Microservice split, observability stack
