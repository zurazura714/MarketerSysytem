# Docker Learning Workbook — MarketerSystem

> Personal learning file — **committed to the repo on purpose** (learning-in-public, user request 2026-07-13).
> Started: 2026-07-07. Docker Desktop 29.4.3 / Compose v5.1.3 installed and verified.
>
> **Progress tracking (standing rule, user request):** the user learns across many different sessions,
> so Claude MUST keep this file current *as progress happens* — tick `[x]` boxes and append a session-log
> row whenever an item is completed, without being asked. When the user says "docker learning",
> read this file and resume at the first unchecked item, in coach mode.

## The Method (rules for every stage)

1. **You type, Claude coaches.** Run commands with the `!` prefix in Claude Code so the output lands in the conversation.
2. **Predict before you run.** Say out loud what you expect the command to do. Compare with reality. The gap is the lesson.
3. **You draft every file first.** Dockerfile and compose files are written by you, reviewed by Claude like a PR. The spec at `docs/superpowers/specs/2026-06-11-docker-design.md` is the answer key — look only *after* your attempt.
4. **Break things on purpose.** Every stage ends with a controlled failure and you diagnosing it.
5. One stage per sitting is fine. 20–60 min each.

## Debugging loop (memorize this order)

`docker ps -a` → `docker logs <name>` → `docker inspect <name>` → `docker exec -it <name> bash`
(what exists? → what did it say? → how is it configured? → look inside)

---

## Stage 0 — Containers by hand `[ ]`

**Goal:** feel the difference between image and container; ports and env vars.

- `[x]` `docker run hello-world` — predict first: what will Docker do if the image isn't local? *(done 2026-07-08: saw pull-then-run, layer downloads)*
- `[ ]` Run SQL Server manually (type it, don't paste):
  ```
  docker run --name learn-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<Strong!Pass1>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
  ```
  Explain to Claude what every flag does before running.
- `[x]` `docker ps` vs `docker ps -a` — what's the difference? *(done 2026-07-08: running vs exists; Exited (0); auto-generated names; removed with docker rm)*
- `[ ]` `docker logs learn-sql` — find the line proving SQL Server is ready.
- `[ ]` Connect SSMS to `localhost,1433` (user `sa`) — create a database `LearnDB` with one table, one row.
- `[ ]` **Controlled failure:** run the same command again with a weak password (`abc`). Diagnose why the container exits using the debugging loop. (SQL Server enforces password complexity — the container just dies.)
- `[ ]` Quiz me: image vs container; what `-d` does; where the container's data lives right now.

## Stage 1 — Volumes: watch data die, then survive `[ ]`

**Goal:** why volumes exist — felt, not read.

- `[ ]` `docker rm -f learn-sql`, start it again, look for `LearnDB` in SSMS. Gone. Why?
- `[ ]` Restart with `-v learn-sql-data:/var/opt/mssql`. Recreate `LearnDB`. `rm -f` and start again **with the same -v**. Data survives.
- `[ ]` `docker volume ls`, `docker volume inspect learn-sql-data` — where does Windows actually keep this?
- `[ ]` Cleanup: `docker rm -f learn-sql`, `docker volume rm learn-sql-data`.
- `[ ]` Quiz: container filesystem vs volume; when does `docker rm` lose data and when not.

## Stage 2 — Naive Dockerfile for the API `[ ]`

**Goal:** build context, layers, COPY, ENTRYPOINT — by writing a deliberately crude version.

- `[ ]` Write a **single-stage** Dockerfile yourself (SDK image, `COPY . .`, `dotnet publish`, `ENTRYPOINT`). No peeking at the spec.
- `[ ]` Create a minimal `.dockerignore` (`**/bin`, `**/obj`, `.git/`) — first build without it, then with it; compare build context size in the build output.
- `[ ]` `docker build -t marketer-api:naive .` — read every line of output; ask Claude about anything unclear.
- `[ ]` `docker image ls` — note the size (expect ~1 GB+). Write it here: ______
- `[ ]` Run it: `docker run -p 8080:8080 marketer-api:naive` — **it will crash or misbehave** (no database reachable). Diagnose with logs; understand *why* (LocalDB connection string points nowhere inside a container).
- `[ ]` Quiz: what is the build context; why `.dockerignore` matters; what a layer is.

## Stage 3 — Multi-stage + chiseled: make it good `[ ]`

**Goal:** layer caching and image size — measured, not believed.

- `[ ]` Rewrite as multi-stage: SDK stage builds, `aspnet:10.0-noble-chiseled` runs. Your draft first, then compare against the spec's Dockerfile and reconcile differences.
- `[ ]` Understand the csproj-copy-then-restore trick: change one line of C# code, rebuild, watch restore come from cache. Then touch a csproj, rebuild, watch restore re-run.
- `[ ]` Measure: naive size ______ vs multi-stage size ______ (expect ~120–250 MB). Rebuild time after code-only change: ______
- `[ ]` Quiz: why copy csprojs before source; what chiseled means (no shell — try `docker exec -it <c> bash`, watch it fail); why `/p:UseAppHost=false`.

## Stage 4 — Compose: the real deliverable `[ ]`

**Goal:** container DNS, healthchecks, env-var config. This stage's output IS the Month-1 roadmap deliverable.

- `[ ]` Write `docker-compose.yml` yourself: `db` (SQL 2022 + volume + healthcheck) and `web` (build: ., depends_on healthy, env vars). Compare with spec afterward.
- `[ ]` Create `.env` + `.env.example`; confirm `.env` is gitignored (`git check-ignore .env`).
- `[ ]` Understand the connection string: why `Server=db,1433` works (compose network DNS) and why `ConnectionStrings__MarketerDBContext` (double underscore) overrides appsettings.json.
- `[ ]` `docker compose up --build` — watch the ordering: db → healthy → web → migrations apply.
- `[ ]` Verify: `/healthz` 200, `/scalar/v1` renders, `GET /api/Distributor` returns the 2 seeded distributors.
- `[ ]` `docker compose down -v` then `up` again — clean re-seed proves migrations work from scratch. (Note: our deterministic-seed fix is what makes this work in UTC containers — ask Claude for the war story.)
- `[ ]` Quiz: what compose adds over `docker run`; service name vs container name; healthcheck vs depends_on.

## Stage 5 — Break & fix drills `[ ]`

**Goal:** the debugging loop becomes reflex. Do these cold, no notes.

- `[ ]` Sabotage 1: wrong `DB_PASSWORD` in `.env`. Predict the failure mode, then diagnose from scratch.
- `[ ]` Sabotage 2: `docker stop <db>` while web is running, hit `/api/Distributor` and `/healthz`. What breaks, what does the health check report?
- `[ ]` Sabotage 3: comment out the healthcheck, `docker compose up` from cold. Race condition — web starts before SQL is ready. What error appears?
- `[ ]` Sabotage 4 (Claude picks): Claude secretly breaks one thing in compose/env; you diagnose it live.
- `[ ]` Housekeeping: `docker system df`, `docker system prune` — understand what's safe to delete.

## Graduation checklist

- `[ ]` Can explain image/container/layer/volume/network to a rubber duck without notes.
- `[ ]` Can rebuild the whole stack from `git clone` + `.env` in one command.
- `[ ]` Commit the Docker artifacts (Dockerfile, compose, .dockerignore, .env.example, README section) — Month 1 done.
- `[ ]` Tell Claude to update CLAUDE.md + CLAUDE-docker.md with what shipped.

## Session log

| Date | Stage | Notes / aha-moments |
|---|---|---|
| 2026-07-08 | 0 (in progress) | docker run = pull+create+start; container lives as long as its process; ps vs ps -a; Exited (0) = clean exit code. Next: run SQL Server with --name/-e/-p/-d (flag explanations pending). |
