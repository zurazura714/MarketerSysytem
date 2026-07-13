# Microservices Analysis — MarketerSystem

> Knowledge file — committed to the repo (user request 2026-07-13, learning purposes). Analysis date: 2026-06-12.
> Status: **analysis only — nothing implemented**. Direction agreed with the user; revisit when Month 4 starts.

## Agreed direction (the verdict)

**Full split is the end state the user wants — but gated on domain growth.** Today's domain is too thin to justify four services (CRUD shells with one FK each would be résumé-driven design). The agreed path:

1. **Grow functionality first** so each future service owns real behavior (candidates below).
2. **Extract the Payout Engine first** — the one seam that already exists and is defensible today (Month 4, after Month 3 delivers messaging + outbox).
3. **Split the rest along the seams the new functionality creates**, one service at a time, each with its own justification.

Interview-ready framing the user should be able to give: *"Microservices are an organizational scaling tool first and a technical one second. I grew the domain until the boundaries were real, extracted the payout engine first because its data dependencies travel in events, and split further only where a service owned genuine behavior."*

## Coupling map (as of 2026-06-12 codebase)

| Candidate boundary | Owns | Coupling reality |
|---|---|---|
| Distributor management | Distributor + Passport/Address/ContactInfo/Picture, MLM graph (`RecomendatorID`, `GenerationLinker`) | Core; everything references `DistributorID`; children are FK-bound, reads compose via 4 Includes (`ListWithDetailsAsync`) |
| Product catalog | Product (name, price) | Trivial today; `Sell` snapshots price at creation → catalog is *not* needed downstream |
| Sales | Sell (price snapshot, `SoldDate`, `UsedForPayment`) | Needs Product price once + Distributor existence check at creation only |
| Payout engine | BonusPayment, `BonusPercentages`, `GenerationChain` walk | Needs sales-in-period + seller's upline chain — both can travel in an event |

### Two existing patterns that are accidentally microservice-friendly

1. **`GenerationLinker` denormalizes the whole upline onto the distributor.** The payout calculation parses a string (`GenerationChain.Parse`), never joins the graph. A `SellCreated` event can carry the seller's chain → payout service needs no Distributor DB.
2. **`Sell` snapshots `ProductPrice`/`ProductTotalPrice` at creation.** Payouts read the snapshot, never the catalog. (Side effect: chain + price frozen at sale time is arguably *more correct* — later recomendator/price changes can't retroactively alter past payouts.)

### What fights a split

- Single bounded context today (distributors-sell-products-earn-bonuses is one business story).
- Atomicity: `GenerateBonusPaymentsForPeriodAsync` is atomic because one DbContext commits sale-flagging + payments together. Distribution trades this for idempotent consumers + outbox/inbox + reconciliation.
- Reads: `GET /api/Distributor` is one `AsSplitQuery` today; across services it's API composition.
- No scaling pressure, single developer — the *organizational* argument for microservices is absent; only the portfolio-demonstration argument applies.

## Functionality candidates (to make the full split real)

Add these to the monolith first (each later anchors a service):

| Functionality | Why it creates a real boundary | Future service |
|---|---|---|
| **Quantity/orders on sales** (fixes the known 3-identical-price-columns issue: `ProductTotalPrice = UnitPrice × Qty`) + inventory/stock levels | Stock reservation, availability checks — different consistency profile than CRUD | Sales/Inventory |
| **Notifications** (email/SMS on payout generated, on new downline recruit) | Pure async consumer, zero coupling back — textbook messaging citizen | Notification service |
| **Auth/Identity** (wire real Entra ID, `[Authorize]`, distributor self-service accounts) | Identity is the classic separately-owned concern | Identity (or stay platform-provided — Entra may make this a non-service; decide later) |
| **Reporting/analytics** (downline performance, payout summaries, leaderboards) | Read-optimized models, CQRS-ish projections off events | Reporting service |

## Target end-state sketch (Month 4+, NOT now)

```
                 ┌──────────────┐  SellCreatedV1   ┌────────────────┐
  Clients ──────►│  Monolith     │ ──── outbox ───► │ PayoutService  │
  (or YARP       │  (Distributors│      (bus)       │  own DB        │
   gateway later)│   Sales/Inv,  │ ◄─ PayoutCreated │  inbox = idem- │
                 │   Catalog)    │                  │  potency       │
                 └──────┬───────┘                   └──────┬─────────┘
                        │            events                │
                        └────────────► NotificationService ◄┘
```

- **Extraction order:** Payout engine → Notifications → Sales/Inventory (only if inventory functionality landed) → Reporting projections. Stop when justification runs out.
- **Event contract v1:** `SellCreatedV1 { SellId, DistributorId, GenerationLinker, ProductTotalPrice, SoldDate }` — versioned from day one.
- **Idempotency:** `UsedForPayment` flag becomes an inbox table keyed by `SellId` in PayoutService.
- **Period generation** becomes event-driven accumulation or a scheduled job inside PayoutService.
- **Shared policy code** (`BonusPercentages`, `GenerationChain`): small contracts package or deliberate duplication — decide at extraction time; do NOT share the Domain assembly.
- **Observability:** correlation IDs in events, OpenTelemetry across the hop.

## Prerequisites already in the roadmap

- Month 1 Docker: compose grows from 2 → N containers naturally (see CLAUDE-docker.md).
- Month 3: RabbitMQ/Service Bus + **Outbox pattern in the monolith** — this is the load-bearing prerequisite for any extraction.

## Known risks / honest caveats

- Eventual consistency: payouts won't exist the instant a sale lands (fine for this domain — payouts are period-based anyway).
- A full split done *before* the functionality exists would be 4 anemic services — the user explicitly chose to grow functionality first to avoid this.
- Each extraction must keep the monolith deployable and tests green; extraction is replace-by-strangler, not big-bang.
