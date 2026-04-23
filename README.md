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

Active development — Sprint 3.3 completed

Current capabilities include:

- Python-based transaction ingestion
- PostgreSQL persistence
- ASP.NET Core API analytics endpoints
- Razor Pages UI with interactive filtering
- category-based analytics
- visual analytics with charts

## Features

### Data Ingestion

- CSV ingestion pipeline (Python)
- Data normalization and validation
- Rule-based transaction categorization

### Backend API

- Transaction retrieval from PostgreSQL
- Financial summary calculations
- Category-based filtering
- Category analytics endpoints
- Trend analytics endpoint for time-series visualization

### UI (Razor Pages)

- Transaction table view
- Summary dashboard (income, expenses, net balance)
- Category filtering via dropdown
- Category analytics overview
- Category expense pie chart
- Income vs expense trend line chart

### Analytics Capabilities

- Income vs expenses overview
- Category-level breakdown of spending
- Interactive filtering using query parameters
- Time-series visualization of financial activity