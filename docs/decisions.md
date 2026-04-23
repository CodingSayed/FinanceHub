# Technical Decisions

## Why Python for ingestion?

Python was chosen for the ingestion layer because it provides:

- strong CSV and tabular data tooling
- rapid ETL development
- straightforward validation workflows

## Why ASP.NET Core for the API?

ASP.NET Core was chosen because it provides:

- strong typing
- maintainable backend architecture
- solid integration with the existing development experience

## Why Razor Pages for the UI?

Razor Pages was chosen because it:

- fits the project scope well
- keeps the full stack cohesive
- allows fast development with clean server-rendered pages

## Why PostgreSQL?

PostgreSQL was chosen because it:

- is strong for relational and analytical workloads
- works well in containerized local development
- is highly relevant for portfolio projects

## Why separate raw and normalized transaction models?

A distinction was made between raw input rows and normalized transaction records to keep the ingestion pipeline explicit and maintainable.

This makes it easier to:

- support multiple bank CSV formats later
- isolate parsing and normalization concerns
- apply validation after standardization

## Why track data quality issues explicitly?

Data quality issues are modeled explicitly so the ingestion process can:

- continue processing valid rows
- surface invalid rows in a structured way
- support future import batch reporting
- make the ETL pipeline easier to debug and extend

## Why introduce import batches and ingestion results early?

Import batches and result summaries were introduced early to ensure that ingestion runs are explicit, traceable, and extensible.

This will make it easier later to:

- persist import metadata to the database
- show import history in the UI
- track data quality metrics per run

## Why implement category analytics in the API layer?

### Context

Category information was already available in the database after Sprint 3.1, but no analytical capabilities were exposed.

A decision was required on where to implement:

- filtering logic
- aggregation logic

### Decision

Category filtering and aggregation are implemented in the API service layer (`TransactionService`) using raw SQL.

### Rationale

- SQL is well-suited for aggregation and filtering
- keeps the UI simple and focused on rendering
- avoids duplicating logic across layers
- aligns with separation of concerns:
  - ingestion = data creation
  - API = data querying and analytics
  - UI = presentation

### Consequences

**Positive:**

- efficient queries executed directly in PostgreSQL
- clear and maintainable architecture
- easy to extend with new analytics endpoints

**Negative:**

- service layer contains SQL logic (no abstraction layer yet)
- may require refactoring if complexity increases significantly
