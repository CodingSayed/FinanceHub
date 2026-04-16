import psycopg2
from psycopg2.extras import execute_batch

from financehub_ingestion.models.import_batch import ImportBatch
from financehub_ingestion.models.ingestion_result import IngestionResult


def save_batch(connection, batch: ImportBatch) -> int:
    with connection.cursor() as cursor:
        cursor.execute(
            """
            INSERT INTO import_batches (source_name, file_name, started_at_utc)
            VALUES (%s, %s, %s)
            RETURNING id
            """,
            (batch.source_name, batch.file_name, batch.started_at_utc),
        )
        batch_id = cursor.fetchone()[0]
        return batch_id


def save_transactions(connection, batch_id: int, records):
    with connection.cursor() as cursor:
        execute_batch(
            cursor,
            """
            INSERT INTO transactions (transaction_date, description, amount, currency, source, category, batch_id)
            VALUES (%s, %s, %s, %s, %s, %s, %s)
            """,
            [
                (
                    (
                        r.transaction_date,
                        r.description,
                        r.amount,
                        r.currency,
                        r.source,
                        r.category,
                        batch_id,
                    )
                )
                for r in records
            ],
        )


def save_quality_issues(connection, batch_id: int, issues):
    with connection.cursor() as cursor:
        execute_batch(
            cursor,
            """
            INSERT INTO data_quality_issues (row_number, raw_description, issue_type, message, batch_id)
            VALUES (%s, %s, %s, %s, %s)
            """,
            [
                (
                    i.row_number,
                    i.raw_description,
                    i.issue_type,
                    i.message,
                    batch_id,
                )
                for i in issues
            ],
        )


def save_ingestion_result(connection, batch: ImportBatch, result: IngestionResult):
    batch_id = save_batch(connection, batch)

    save_transactions(connection, batch_id, result.successful_records)
    save_quality_issues(connection, batch_id, result.quality_issues)

    connection.commit()