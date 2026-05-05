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

## Implemented in Sprint 3.3

The application now includes visual analytics capabilities in the UI, backed by dedicated trend data from the API.

### API Enhancements

The API now exposes a trend endpoint:

- retrieve income and expense trends over time:
  - `GET /api/transactions/trends`

This endpoint is implemented in the existing `TransactionService` using raw SQL aggregation grouped by transaction date.

The trend response includes:

- date
- income
- expense
- net balance

### UI Enhancements

The Razor Pages UI now supports chart-based analytics:

- category expense breakdown pie chart
- income vs expense trend line chart

The UI consumes API-provided analytical data and renders it using Chart.js.

### Architectural Impact

This extends FinanceHub from tabular analytics to visual analytics.

The architecture now supports:

- ingestion and categorization in Python
- analytical read endpoints in the API
- chart rendering in the UI

This creates a stronger foundation for future dashboard-style functionality, including:

- richer chart layouts
- monthly trend analysis
- comparative analytics across categories and periods

## Implemented in Sprint 3.4

The ingestion layer now uses a more realistic transaction dataset.

### Ingestion Enhancements

Sprint 3.4 introduced:

- a larger realistic CSV dataset
- recurring income and expenses
- broader transaction descriptions
- intentional data quality issues
- improved rule-based categorization

### Architectural Impact

The system now better represents real-world ingestion scenarios.

The ingestion layer continues to handle:

- normalization
- validation
- categorization
- persistence
- data quality issue tracking

This makes the platform more realistic and prepares it for future support of external bank exports or larger datasets.

## Implemented in Sprint 4

The UI layer now includes a structured dashboard layout for financial analytics.

### UI Enhancements

Sprint 4 introduced:

- dashboard-style layout using grid-based structure
- summary cards with consistent styling
- side-by-side chart layout (pie chart + line chart)
- responsive behavior for smaller screens
- consistent formatting for financial values

### Architectural Impact

The UI has evolved from a simple data display into a structured analytics dashboard.

The architecture now supports:

- separation between data retrieval and presentation layout
- reusable styling through CSS instead of inline styles
- scalable dashboard structure for future analytics features

This prepares the UI layer for:

- additional filters (e.g. date range)
- more complex dashboards
- extended analytical views

## Implemented in Sprint 4

The API and UI layers now support date-based filtering.

### API Enhancements

The API now supports filtering transactions and summaries by date range:

- filter transactions by date range:
  - GET /api/transactions?startDate=2024-01-01&endDate=2024-01-10
- filter financial summary by date range:
  - GET /api/transactions/summary?startDate=2024-01-01&endDate=2024-01-10

Date filtering is implemented in the TransactionService using SQL WHERE clauses.

### UI Enhancements

The Razor Pages UI now supports:

- date range filtering via query parameters
- filtered transactions based on selected date range
- filtered summary metrics
- charts that respond dynamically to date filters

### Architectural Impact

The system now supports multi-dimensional filtering:

- category-based filtering (Sprint 3.2)
- time-based filtering (Sprint 4)

This enables more advanced analytical exploration and prepares the system for dashboard-style use cases.

## Implemented in Sprint 4.1

The API and UI layers now support advanced data filtering and pagination.

### API Enhancements

The API now supports:

- date range filtering:
  - `GET /api/transactions?startDate=2024-01-01&endDate=2024-01-31`
- pagination:
  - `GET /api/transactions?page=1&pageSize=10`

Filtering parameters can be combined:

- category + date range + pagination

All filtering and pagination logic is implemented in the `TransactionService` using SQL.

### UI Enhancements

The Razor Pages UI now supports:

- date range filtering via input fields
- combined filters (category + date range)
- pagination controls (Previous / Next)
- persistent filters across page navigation

### Architectural Impact

This extends the system from static analytics to interactive data exploration.

The API now supports:

- filtered queries
- paginated data access

The UI now behaves more like a real dashboard application.