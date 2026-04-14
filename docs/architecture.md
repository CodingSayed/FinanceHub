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
