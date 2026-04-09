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
