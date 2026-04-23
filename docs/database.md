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
  