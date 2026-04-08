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