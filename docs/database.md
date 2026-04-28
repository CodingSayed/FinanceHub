# Database

## Engine

PostgreSQL

## Planned tables

- transactions
- categories
- merchants
- budgets
- import_batches
- data_quality_issues

## Design Direction

The primary storage model will be relational and normalized.

Analytics-oriented views or summary tables may be added later for:

- monthly spend summaries
- budget versus actual comparisons
- category trend analysis

## Implemented in Sprint 2.1

The ingestion pipeline now persists:

- import batches
- normalized transactions
- data quality issues

All data is stored in PostgreSQL using raw SQL inserts.

## Implemented in Sprint 3.1

The `transactions` table now includes a category column:

    ALTER TABLE transactions
    ADD COLUMN IF NOT EXISTS category TEXT;

This column is populated during ingestion using rule-based categorization.

## Implemented in Sprint 3.2

The category column is now actively used for analytics in both the API and the UI.

### Usage in API

The API uses the category column for:

- filtering transactions
- filtering financial summaries
- grouping transactions by category

Example filter pattern:

    WHERE category = @category

### Aggregations

Category-based aggregations include:

- total income per category
- total expenses per category
- net balance per category
- transaction count per category

### Notes

- Empty or NULL categories are handled using:
  
      COALESCE(NULLIF(category, ''), 'Uncategorized')

- Category values originate from the ingestion layer
- Sprint 3.2 introduced both:
  - API endpoints for category analytics
  - UI support for category filtering and category summary rendering
  
## Implemented in Sprint 3.3

The existing `transactions` table is now also used as the source for time-series analytics.

### Trend Analytics Usage

The API aggregates transaction data by `transaction_date` to support trend visualization.

This includes:

- total income per day
- total expense per day
- net balance per day

### Example Analytical Use

The API trend endpoint is based on grouped reads over the `transactions` table, using the transaction date as the time dimension.

Example grouping pattern:

    GROUP BY transaction_date
    ORDER BY transaction_date

### Notes

- No new tables were required for Sprint 3.3
- Trend analytics are computed dynamically from transactional data
- This keeps the storage layer simple while expanding analytical capability in the API and UI

## Implemented in Sprint 3.4

The database now stores a more realistic transaction dataset.

### Dataset Impact

Sprint 3.4 introduced a larger CSV dataset with:

- recurring income
- recurring expenses
- multiple spending categories
- invalid date examples
- invalid amount examples
- validation threshold examples

### Persistence Behavior

The existing tables continue to support the ingestion flow:

- `import_batches` stores import metadata
- `transactions` stores valid normalized transactions
- `data_quality_issues` stores invalid or rejected records

### Notes

- No schema changes were required for Sprint 3.4
- Existing tables were sufficient for realistic ingestion testing
- The larger dataset makes analytics charts and category summaries more meaningful