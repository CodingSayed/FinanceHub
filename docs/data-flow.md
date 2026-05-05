# Data Flow

## Planned flow

1. CSV files are provided as input.
2. The Python ingestion layer reads the raw files.
3. Data is normalized into a standard transaction format.
4. Validation rules are applied.
5. Cleaned data is stored in PostgreSQL.
6. The .NET API exposes analytics endpoints.
7. The Razor Pages UI visualizes finance insights.

## Current Status

The current implementation includes:

- a Python ingestion entry point
- CSV reading
- PostgreSQL local setup via Docker

## Implemented in Sprint 1.1

The ingestion flow currently supports:

- reading CSV input
- mapping rows to a raw transaction contract
- normalizing rows into a standard transaction record
- validating normalized transactions

## Implemented in Sprint 1.2

The ingestion flow now supports:

- multiple date input formats
- multiple decimal amount formats
- row-level normalization error handling
- row-level validation issue reporting
- explicit data quality issue tracking

## Implemented in Sprint 1.3

The ingestion process now models:

- an import batch
- a pipeline execution step
- an ingestion result summary

This makes each ingestion run easier to track and reason about.

## Implemented in Sprint 2.3

The application now supports the first full end-to-end user-facing flow:

1. CSV input is ingested in Python
2. normalized data is stored in PostgreSQL
3. the ASP.NET Core API exposes transaction data
4. the Razor Pages UI renders transactions in a table

## Implemented in Sprint 2.4

The API now provides financial summaries:

- total income
- total expenses
- net balance

The UI displays these insights above the transaction list.

## Implemented in Sprint 3.1

Transactions are now categorized during ingestion using rule-based logic.

This enables:

- spend analysis
- grouping by category
- future ML-based categorization

## Implemented in Sprint 3.2

The data flow now includes category-based analytics.

### Extended Flow

1. Transactions are categorized during ingestion.
2. Category values are stored in PostgreSQL.
3. The API supports:
   - filtering transactions by category
   - filtering summaries by category
   - retrieving category lists
   - computing category-level aggregations
4. The UI:
   - retrieves categories from the API
   - allows users to select a category filter
   - sends the selected category as a query parameter
   - renders filtered transactions and summaries
   - displays category-level analytics

### Result

Users can now interactively explore financial data based on transaction categories, enabling deeper insight into spending behavior.

## Implemented in Sprint 3.3

The data flow now includes visual analytics based on backend trend data.

### Extended Flow

1. Transactions are stored in PostgreSQL with date, amount, and category data.
2. The API aggregates trend data by transaction date.
3. The API exposes:
   - category summary data
   - time-series trend data
4. The UI retrieves:
   - category summary data for the pie chart
   - trend data for the line chart
5. The UI renders:
   - category expense distribution
   - income vs expense trend over time

### Result

Users can now move beyond static summaries and tables, and visually inspect financial patterns over time and across categories.

## Implemented in Sprint 3.4

The data flow now supports a more realistic transaction dataset.

### Extended Flow

1. A larger CSV file is provided as input.
2. The ingestion layer parses multiple transaction formats.
3. Valid transactions are normalized and categorized.
4. Invalid records are captured as data quality issues.
5. Successful records are stored in PostgreSQL.
6. The API exposes the larger dataset through existing analytics endpoints.
7. The UI visualizes more meaningful category and trend patterns.

### Result

The application now demonstrates a more realistic end-to-end analytics flow, including valid data, invalid data, categorization gaps, and improved category coverage.

## Implemented in Sprint 4

The data flow now supports a structured dashboard presentation layer.

### Extended Flow

1. The API provides transaction, summary, category, and trend data.
2. The UI retrieves all required datasets in a single page load.
3. The UI organizes data into:
   - summary cards
   - category distribution chart
   - trend chart
   - tabular views
4. Chart data is transformed client-side for visualization.

### Result

The UI now presents data in a structured dashboard format, allowing users to quickly interpret financial insights without scanning raw tables.

## Implemented in Sprint 4

The data flow now supports time-based filtering.

### Extended Flow

1. Transactions are stored with a transaction_date.
2. The API applies date filters using:
   - startDate
   - endDate
3. Filter logic is applied at query level in PostgreSQL.
4. The UI sends selected date filters via query parameters.
5. The API returns filtered datasets.
6. The UI renders:
   - filtered transaction list
   - filtered summaries
   - filtered charts

### Result

Users can now analyze financial data within specific time windows, enabling:

- period-based analysis
- short-term vs long-term comparisons
- more meaningful trend insights

## Implemented in Sprint 4.1

The data flow now includes filtered and paginated data retrieval.

### Extended Flow

1. Transactions are stored in PostgreSQL.
2. The API applies:
   - category filters
   - date range filters
   - pagination
3. The API returns a subset of data based on filters.
4. The UI:
   - sends filter parameters via query string
   - preserves filters during pagination
   - updates the view dynamically

### Result

Users can now:

- explore transactions within specific date ranges
- navigate large datasets using pagination
- combine multiple filters for precise analysis