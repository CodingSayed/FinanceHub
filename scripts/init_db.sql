CREATE TABLE IF NOT EXISTS import_batches (
    id SERIAL PRIMARY KEY,
    source_name TEXT NOT NULL,
    file_name TEXT NOT NULL,
    started_at_utc TIMESTAMP NOT NULL
);

CREATE TABLE IF NOT EXISTS transactions (
    id SERIAL PRIMARY KEY,
    transaction_date DATE NOT NULL,
    description TEXT NOT NULL,
    amount NUMERIC(12, 2) NOT NULL,
    currency TEXT NOT NULL,
    source TEXT NOT NULL,
    batch_id INTEGER REFERENCES import_batches(id)
);

CREATE TABLE IF NOT EXISTS data_quality_issues (
    id SERIAL PRIMARY KEY,
    row_number INTEGER NOT NULL,
    raw_description TEXT,
    issue_type TEXT NOT NULL,
    message TEXT NOT NULL,
    batch_id INTEGER REFERENCES import_batches(id)
);