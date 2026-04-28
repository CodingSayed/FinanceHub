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

## Why implement trend analytics in the API layer?

### Context

After category analytics were introduced in Sprint 3.2, the next step was to add time-based visual analytics.

A decision was needed on where to calculate trend data:

- in the UI from raw transactions
- or in the API from aggregated database queries

### Decision

Trend aggregation is implemented in the API layer through a dedicated endpoint:

- `GET /api/transactions/trends`

The aggregation logic remains in `TransactionService` and is executed in SQL.

### Rationale

- keeps analytical logic in the backend
- avoids duplicating aggregation logic in the UI
- makes the UI thinner and easier to maintain
- aligns with the architecture principle that the API owns read and analytics behavior

### Consequences

**Positive:**

- trend logic is centralized
- the UI consumes a clean analytical contract
- the system is easier to extend with future visualizations

**Negative:**

- the service layer continues to grow in responsibility
- future complexity may justify a separate query or analytics layer

## Why use Chart.js for visual analytics?

### Context

Sprint 3.3 required a lightweight way to introduce visual analytics into the Razor Pages UI.

### Decision

Chart.js is used in the UI to render:

- category expense pie chart
- income vs expense line chart

### Rationale

- easy to integrate into the existing server-rendered UI
- lightweight and fast to adopt
- sufficient for current dashboard and analytics needs
- avoids unnecessary frontend framework complexity

### Consequences

**Positive:**

- quick delivery of high-value visual analytics
- minimal architectural overhead
- easy to extend with additional charts

**Negative:**

- chart logic is currently embedded in the page
- more advanced dashboard behavior may require future refactoring


## Why introduce a realistic controlled dataset before using external datasets?

### Context

The initial sample dataset was useful for proving the ingestion and analytics flow, but it was too small to demonstrate realistic financial behavior.

A decision was needed between immediately using an external dataset or first creating a controlled realistic dataset.

### Decision

Sprint 3.4 introduces a controlled realistic CSV dataset before adopting external datasets such as Kaggle or real bank exports.

### Rationale

- keeps the input format aligned with the current ingestion pipeline
- allows intentional testing of data quality scenarios
- avoids privacy concerns from real bank data
- makes debugging easier than starting with an unknown external dataset
- creates more meaningful charts and category summaries

### Consequences

**Positive:**

- more realistic analytics output
- better validation of the ingestion pipeline
- controlled data quality issue testing
- improved categorization coverage

**Negative:**

- dataset is still synthetic
- external dataset compatibility remains future work