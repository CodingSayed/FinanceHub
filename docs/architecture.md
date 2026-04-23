# Architecture

FinanceHub is a hybrid system consisting of:

- Python ingestion layer (ETL)
- ASP.NET Core API (backend)
- Razor Pages UI (frontend)
- PostgreSQL database

## Layers

1. Ingestion (Python)
2. Storage (PostgreSQL)
3. API (.NET)
4. UI (Razor Pages)

## Current Status

Sprint 0 focuses on project setup, local development tooling, and the technical foundation.

## Implemented in Sprint 1.3

The ingestion layer now separates:

- batch metadata
- ingestion execution
- ingestion result reporting

This improves maintainability and prepares the pipeline for future database persistence.

## Implemented in Sprint 2.2

The API layer now reads transaction data from PostgreSQL.

This connects:

- Python ingestion layer
- database storage
- backend API

## Implemented in Sprint 2.3

The UI layer now consumes transaction data through the backend API.

This completes the first end-to-end path across:

- ingestion
- database storage
- API access
- frontend rendering

## Implemented in Sprint 3.2

The API and UI layers now support category-based analytics.

### API Enhancements

The API now exposes additional endpoints:

- filter transactions by category:
  - `GET /api/transactions?category=Groceries`
- filter financial summary by category:
  - `GET /api/transactions/summary?category=Groceries`
- retrieve available categories:
  - `GET /api/transactions/categories`
- retrieve aggregated category summaries:
  - `GET /api/transactions/categories/summary`

All data access and query logic is implemented in the `TransactionService` using raw SQL with Npgsql.

No repository abstraction layer is used. The service layer directly interacts with PostgreSQL.

### UI Enhancements

The Razor Pages UI now supports:

- category-based filtering via dropdown
- filtered transaction views
- filtered summary metrics
- category-level aggregated analytics table

### Architectural Impact

This extends the system from simple data retrieval to analytical capabilities.

The architecture now clearly separates:

- ingestion (Python, write-focused)
- API (read + analytics logic)
- UI (presentation layer)

This lays the foundation for future analytical features such as:

- charting
- trend analysis
- predictive modeling
