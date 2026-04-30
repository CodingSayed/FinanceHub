# FinanceHub

FinanceHub is a hybrid .NET + Python personal finance analytics platform.

## Tech Stack

- ASP.NET Core Web API
- ASP.NET Core Razor Pages
- Python (ETL / ingestion)
- PostgreSQL
- Docker

## Project Structure

- `src/FinanceHub.API` - backend API
- `src/FinanceHub.Ui` - frontend application
- `src/FinanceHub.Ingestion` - Python ingestion layer
- `docs` - project documentation
- `sample-data` - local sample input files

## Documentation

- `docs/architecture.md`
- `docs/data-flow.md`
- `docs/decisions.md`
- `docs/database.md`

## Status

Active development — Sprint 4 (Dashboard UX) in progress

Current capabilities include:

- Python-based transaction ingestion
- PostgreSQL persistence
- ASP.NET Core API analytics endpoints
- Razor Pages UI with interactive filtering
- category-based analytics
- visual analytics with charts
- realistic dataset ingestion with data quality handling

## Features

### Data Ingestion

- CSV ingestion pipeline (Python)
- Data normalization and validation
- Rule-based transaction categorization
- Support for multiple date and amount formats
- Data quality issue detection (invalid date, invalid amount, threshold violations)
- Realistic dataset ingestion for testing and analytics

### Backend API

- Transaction retrieval from PostgreSQL
- Financial summary calculations
- Category-based filtering
- Category analytics endpoints
- Trend analytics endpoint for time-series visualization

### UI (Razor Pages)

- Dashboard-style layout
- Summary cards (income, expenses, net balance)
- Category filtering via dropdown
- Category analytics overview
- Category expense pie chart
- Income vs expense trend line chart
- Responsive layout with charts and cards

### Analytics Capabilities

- Income vs expenses overview
- Category-level breakdown of spending
- Interactive filtering using query parameters
- Time-series visualization of financial activity
- Realistic trend analysis based on larger transaction datasets
- Improved category distribution through enhanced categorization rules
- Structured dashboard visualization for financial insights
