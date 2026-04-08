# Technical Decisions

## Why Python for ingestion?

Python was chosen for the ingestion layer because it provides:
- strong CSV and tabular data tooling
- rapid ETL development
- straightforward validation workflows

## Why ASP.NET Core for the API?

ASP.NET Core was chosen because it provides:
- strong typing
- maintainable backend architecture
- solid integration with the existing development experience

## Why Razor Pages for the UI?

Razor Pages was chosen because it:
- fits the project scope well
- keeps the full stack cohesive
- allows fast development with clean server-rendered pages

## Why PostgreSQL?

PostgreSQL was chosen because it:
- is strong for relational and analytical workloads
- works well in containerized local development
- is highly relevant for portfolio projects