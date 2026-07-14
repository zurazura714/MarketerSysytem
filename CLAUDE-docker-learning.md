# Docker Learning Guide — MarketerSystem (self-study edition)

> Committed to the repo on purpose (learning-in-public). Rewritten 2026-07-13 as a **standalone
> follow-along guide** — you work in your own terminal (PowerShell), no AI session needed.
> Started: 2026-07-07. Docker Desktop 29.4.3 / Compose v5.1.3 installed and verified.
>
> **Progress tracking:** tick `[x]` yourself as you go, add a session-log row at the bottom — OR just
> tell any Claude session **"docker learning, I finished X"** and it will tick the boxes, log the date,
> and commit for you (standing rule in CLAUDE.md). Saying "docker learning" alone resumes at the first
> unchecked item.

## How to use this guide

1. **Predict before you run.** Before every command, say (or write) what you expect. The gap between prediction and reality is the lesson.
2. **Type commands, don't paste.** Muscle memory matters.
3. **Draft every file yourself first** (Dockerfile, compose). The answer key is `docs/superpowers/specs/2026-06-11-docker-design.md` — look only *after* your attempt, then reconcile differences.
4. Every stage ends with a **controlled failure** you diagnose yourself, then a self-quiz (answers at the end of each stage — don't peek early).
5. One stage per sitting, 20–60 min each.

## The debugging loop (memorize — use in this order every time something is wrong)

```
docker ps -a          # what exists, and what state is it in?
docker logs <name>    # what did it say before it died?
docker inspect <name> # how is it actually configured (env, ports, mounts)?
docker exec -it <name> bash   # go inside and look around
```

## Current state (as of 2026-07-14)

- **Stage 0 complete** (finished 2026-07-14). Now working across **two PCs** — the original machine plus a second one (Windows 11).
- Image `mcr.microsoft.com/mssql/server:2022-latest` pulled on both machines (re-pulled on the new PC 2026-07-14, ~1.5 GB).
- Container **`learn-sql` exists on the new PC (ID `6bce5325…`), currently STOPPED.** Resume it with **`docker start learn-sql`** — *not* `docker run` (run only ever creates a *new* container). Exact SA password on this container wasn't captured this session; if a connect fails, `docker rm learn-sql` and recreate with a known one.
- `hello-world` run on the new PC as container `practical_murdock` (2026-07-14).
- (Old PC, 2026-07-13) its `learn-sql` had SA password literally `<ZuraTest1>` **with the angle brackets** — copy-paste accident kept as a lesson: quotes protected `<>` from the shell, so the brackets became part of the password.

---

## Stage 0 — Containers by hand `[x]`

**Goal:** feel the difference between image and container; ports and env vars.

- `[x]` `docker run hello-world` *(done 2026-07-08: docker run = pull + create + start; layer downloads)*
- `[x]` `docker ps` vs `docker ps -a` *(done 2026-07-08: running vs everything; `Exited (0)` = clean exit; auto-generated names; `docker rm` removes)*
- `[x]` Run SQL Server manually *(done 2026-07-13, container `learn-sql`)*:
  ```
  docker run --name learn-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong!Pass1>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
  ```
  **What each flag does:**
  - `--name learn-sql` — human-chosen container name (otherwise Docker invents one like `wonderful_bhabha`); used in every later command (`logs`, `rm`, `exec`).
  - `-e KEY=VALUE` — sets an environment variable **inside** the container. This is how containers are configured — same image, different behavior. `ACCEPT_EULA=Y` accepts the license; `MSSQL_SA_PASSWORD` sets the `sa` login password.
  - `-p 1433:1433` — port publishing, format `host:container`. Traffic to `localhost:1433` on Windows is forwarded to port 1433 inside the container. Without `-p`, the container runs but nothing outside can reach it.
  - `-d` — detached: runs in the background and prints the container ID instead of tying up your terminal with SQL Server's output.
- `[x]` Verify it's running: `docker ps` — expect STATUS `Up ...` and PORTS `0.0.0.0:1433->1433/tcp`. It also appears in Docker Desktop → Containers.
- `[x]` `docker logs learn-sql` — scroll for the line proving readiness:
  `SQL Server is now ready for client connections. This is an informational message...`
  (If instead the container is gone from `docker ps`, run the debugging loop — most likely the password failed complexity and the process exited.)
- `[x]` Connect **SSMS** → server name `localhost,1433` (comma, not colon), SQL auth, user `sa`, password `<ZuraTest1>` **with the brackets**. Create database `LearnDB`, one table, insert one row:
  ```sql
  CREATE DATABASE LearnDB;
  USE LearnDB;
  CREATE TABLE Note (Id INT PRIMARY KEY, Text NVARCHAR(100));
  INSERT INTO Note VALUES (1, N'hello from a container');
  ```
- `[x]` **Controlled failure:** run a second container with a weak password:
  ```
  docker run --name weak-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=abc" -p 1434:1433 -d mcr.microsoft.com/mssql/server:2022-latest
  ```
  `docker run` succeeds (prints an ID!) but seconds later the container dies. Prove it with the debugging loop: `docker ps -a` shows `Exited (1)`, `docker logs weak-sql` shows the password-complexity error. Lesson: **`-d` returning an ID only means "started", not "healthy"** — always check logs. Clean up: `docker rm weak-sql`.
- `[x]` Self-quiz (answers below — try first):
  1. Image vs container?
  2. What does `-d` change, and what does it NOT guarantee?
  3. Where does LearnDB's data physically live right now?

<details><summary>Stage 0 quiz answers</summary>

1. Image = read-only template (layers, downloaded once). Container = a running (or stopped) *instance* of an image with its own writable layer. One image → many containers.
2. `-d` detaches — the container runs in the background. It does NOT guarantee the process inside is healthy; it can die one second later. Check `docker ps` + `logs`.
3. In the container's **writable layer** (inside Docker Desktop's VM disk). It is deleted forever when the container is removed — that's Stage 1's whole point.
</details>

## Stage 1 — Volumes: watch data die, then survive `[ ]`

**Goal:** why volumes exist — felt, not read.

- `[ ]` Kill and recreate: `docker rm -f learn-sql`, run the same `docker run` again, connect SSMS → **LearnDB is gone.** Why? The writable layer died with the container.
- `[ ]` Recreate **with a volume** (one new flag):
  ```
  docker run --name learn-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<ZuraTest1>" -p 1433:1433 -v learn-sql-data:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2022-latest
  ```
  `-v name:/path` mounts a Docker-managed volume over the container path where SQL Server keeps `.mdf`/`.ldf` files. Recreate LearnDB. Then `docker rm -f learn-sql` and run again **with the same `-v`** → LearnDB survives.
- `[ ]` `docker volume ls` and `docker volume inspect learn-sql-data` — note the Mountpoint (it lives inside Docker Desktop's Linux VM, not directly on C:).
- `[ ]` Cleanup: `docker rm -f learn-sql` then `docker volume rm learn-sql-data`.
- `[ ]` Self-quiz: when does `docker rm` lose data and when not? What's the difference between the container filesystem and a volume?

<details><summary>Stage 1 quiz answers</summary>

`docker rm` destroys the container's writable layer — anything written to a **volume-mounted path** survives because the volume is a separate object with its own lifecycle (`docker volume rm` deletes it). Container FS = ephemeral, tied to the container; volume = persistent, attachable to the next container.
</details>

## Stage 2 — Naive Dockerfile for the API `[ ]`

**Goal:** build context, layers, COPY, ENTRYPOINT — by writing a deliberately crude version.

- `[ ]` Write a **single-stage** `Dockerfile` at the repo root yourself. Skeleton to aim for (fill in, don't paste): FROM the .NET **SDK** image (`mcr.microsoft.com/dotnet/sdk:10.0`) → `WORKDIR /src` → `COPY . .` → `RUN dotnet publish MarketerSysytem.Web -c Release -o /app` → `WORKDIR /app` → `ENTRYPOINT ["dotnet", "MarketerSysytem.Web.dll"]`. No peeking at the spec.
- `[ ]` First build **without** a `.dockerignore`: `docker build -t marketer-api:naive .` — note the "transferring context" size in the first output lines (it ships `bin/`, `obj/`, `.git/` — huge). Then create `.dockerignore` (`**/bin`, `**/obj`, `.git/`) and rebuild — compare context size.
- `[ ]` Read every line of the build output — each `RUN`/`COPY` = one **layer**.
- `[ ]` `docker image ls` — write the naive size here: ______ (expect ~1 GB+, because the SDK image ships compilers you don't need at runtime).
- `[ ]` Run it: `docker run --rm -p 8080:8080 -e ASPNETCORE_URLS=http://+:8080 marketer-api:naive` — **it will crash**: the connection string points at `(localdb)\mssqllocaldb`, which doesn't exist inside a Linux container. Read the exception in the output and understand it. That's Stage 4's problem to solve.
- `[ ]` Self-quiz: what exactly is "the build context"? Why does `.dockerignore` matter? What is a layer?

<details><summary>Stage 2 quiz answers</summary>

Build context = the directory tree sent to the Docker engine when you run `docker build .` — `COPY` can only see files inside it. `.dockerignore` shrinks it (faster builds, no secrets/junk in the image, better cache behavior). A layer = the filesystem diff produced by one Dockerfile instruction; layers are cached and reused when their inputs haven't changed.
</details>

## Stage 3 — Multi-stage + chiseled: make it good `[ ]`

**Goal:** layer caching and image size — measured, not believed.

- `[ ]` Rewrite as **multi-stage**: stage 1 `FROM sdk:10.0 AS build` (restore + publish with `/p:UseAppHost=false`), stage 2 `FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled` + `COPY --from=build`. Your draft first, then compare with the spec's Dockerfile and reconcile every difference.
- `[ ]` The **csproj-copy-then-restore trick**: copy only `Directory.Build.props`, `Directory.Packages.props` and the 7 `.csproj` files, run `dotnet restore`, and *only then* `COPY . .`. Test it: change one line of C#, rebuild → restore layer says `CACHED`. Touch a csproj, rebuild → restore re-runs. Watch it happen.
- `[ ]` Measure: naive ______ vs multi-stage ______ (expect ~120–250 MB). Rebuild time after a code-only change: ______
- `[ ]` Prove "chiseled = no shell": `docker exec -it <container> bash` → fails, there is no bash. Smaller attack surface, smaller image.
- `[ ]` Self-quiz: why copy csprojs before source? Why `/p:UseAppHost=false`? What does chiseled remove?

<details><summary>Stage 3 quiz answers</summary>

Csprojs change rarely, source changes constantly — splitting them lets the expensive `dotnet restore` layer stay cached across code edits. `/p:UseAppHost=false` skips the native launcher executable (useless in a container; `ENTRYPOINT ["dotnet", "x.dll"]` runs it). Chiseled Ubuntu strips shell, package manager, and root user — only the runtime + your app remain.
</details>

## Stage 4 — Compose: the real deliverable `[ ]`

**Goal:** container DNS, healthchecks, env-var config. **This stage's output IS the Month-1 roadmap deliverable.**

- `[ ]` Remove `app.UseHttpsRedirection()` from `Program.cs` (container is HTTP-only; TLS terminates at the platform edge in Month 2).
- `[ ]` Write `docker-compose.yml` yourself: service `db` (SQL 2022 image, volume, `MSSQL_SA_PASSWORD=${DB_PASSWORD}`, healthcheck using `sqlcmd -C`), service `web` (`build: .`, `depends_on: db: condition: service_healthy`, ports `8080:8080`). Compare with the spec afterward.
- `[ ]` Create `.env` (real password) + `.env.example` (placeholder). Add `.env` to `.gitignore`; verify with `git check-ignore .env`.
- `[ ]` Understand the connection string override:
  `ConnectionStrings__MarketerDBContext=Server=db,1433;Database=MarketerDB;User Id=sa;Password=${DB_PASSWORD};TrustServerCertificate=True`
  — `db` resolves via the compose network's internal DNS (service name = hostname); the double underscore `__` is ASP.NET Core's env-var syntax for nested config keys, and env vars override `appsettings.json`.
- `[ ]` `docker compose up --build` — watch the ordering: db starts → healthcheck passes → web starts → EF migrations apply + seed.
- `[ ]` Verify: `http://localhost:8080/healthz` = 200, `/scalar/v1` renders, `GET /api/Distributor` returns the 2 seeded distributors.
- `[ ]` `docker compose down -v` then `up` again — clean re-seed from scratch proves migrations work. (This only works because the seeds are deterministic with explicit `TimeSpan.Zero` offsets — the war story is in `CLAUDE-docker.md`.)
- `[ ]` Self-quiz: what does compose add over `docker run`? Service name vs container name? Healthcheck vs `depends_on`?

<details><summary>Stage 4 quiz answers</summary>

Compose = declarative multi-container: one file defines services, network, volumes, env — `up`/`down` manage the whole stack, and services get DNS names. Service name (`db`) is the stable network hostname; container name is the runtime instance label. `depends_on` alone only orders *starting*; with `condition: service_healthy` it waits for the healthcheck to actually pass — without it, web races SQL Server's ~15 s startup and crashes.
</details>

## Stage 5 — Break & fix drills `[ ]`

**Goal:** the debugging loop becomes reflex. Do these cold, no notes.

- `[ ]` Sabotage 1: wrong `DB_PASSWORD` in `.env`. Predict the failure mode, then diagnose from scratch.
- `[ ]` Sabotage 2: `docker stop` the db container while web runs; hit `/api/Distributor` and `/healthz`. What breaks, what does the health check report?
- `[ ]` Sabotage 3: comment out the healthcheck, `docker compose up` from cold. Race condition — what error appears in web's logs?
- `[ ]` Housekeeping: `docker system df`, then `docker system prune` — read the confirmation prompt carefully; understand what's safe to delete (stopped containers, dangling images, unused networks — NOT named volumes unless `--volumes`).

## Graduation checklist

- `[ ]` Explain image/container/layer/volume/network to a rubber duck without notes.
- `[ ]` Rebuild the whole stack from `git clone` + `cp .env.example .env` in one command.
- `[ ]` Commit the Docker artifacts (Dockerfile, docker-compose.yml, .dockerignore, .env.example, README "Run with Docker" section) — **Month 1 done.**
- `[ ]` Update CLAUDE.md + CLAUDE-docker.md with what shipped (tell Claude, or do it yourself).

## Command cheat sheet

```
docker ps / ps -a                 running / all containers
docker logs -f <name>             follow logs
docker exec -it <name> bash       shell inside (not on chiseled)
docker rm -f <name>               force-remove container
docker image ls / volume ls       list images / volumes
docker build -t name:tag .        build image from Dockerfile
docker compose up --build / down -v   stack up (rebuild) / down (+wipe volumes)
docker system df / prune          disk usage / cleanup
```

## Session log

| Date | Stage | Notes / aha-moments |
|---|---|---|
| 2026-07-08 | 0 (in progress) | docker run = pull+create+start; container lives as long as its process; ps vs ps -a; Exited (0) = clean exit code. |
| 2026-07-13 | 0 (in progress) | learn-sql SQL Server container created (pull ≈1.5 GB, container only appears after pull completes). Password literally `<ZuraTest1>` with brackets — quotes protected `<>` from the shell. Guide rewritten for self-study (token saving); flags explained inline. Next: docker ps verify → logs ready-line → SSMS + LearnDB → weak-password drill → quiz. |
| 2026-07-14 | **0 — complete** ✅ | Stage 0 done on the 2nd PC (Windows 11); it was fun. Aha-moments: **image = class, container = instance** (one image → many containers; a container adds its own writable layer). **`docker run` always creates a NEW container** — to bring back a stopped one use **`docker start`**, never `run` (hit "image not found" from naming a container as an image, then a name-conflict on re-create). **Flags are per-subcommand:** `-a` = `--all` on `ps` but `--attach` on `run`/`start` (on `run`, `-a` even eats the next word as a stream name). **`-d`** = background; a returned container ID means "started", NOT "healthy". **`docker exec … bash`** needs the container *running* AND the image to *ship a shell* — fails on hello-world (`FROM scratch`) and chiseled. **Data in the writable layer survives stop/start but dies on `docker rm`** → the hook into Stage 1 (volumes). Quiz passed. |
