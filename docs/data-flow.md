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
