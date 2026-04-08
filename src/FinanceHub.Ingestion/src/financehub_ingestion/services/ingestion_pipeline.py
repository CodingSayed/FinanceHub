from financehub_ingestion.models.data_quality_issue import DataQualityIssue
from financehub_ingestion.models.ingestion_result import IngestionResult
from financehub_ingestion.models.raw_transaction_row import RawTransactionRow
from financehub_ingestion.models.import_batch import ImportBatch
from financehub_ingestion.normalizers.transaction_normalizer import normalize_transaction_row
from financehub_ingestion.readers.csv_reader import read_csv
from financehub_ingestion.validators.transaction_validator import validate_transaction


def run_ingestion(batch: ImportBatch, file_path: str) -> IngestionResult:
    """
    Executes the ingestion pipeline for a given import batch and file.

    Args:
        batch: The import batch metadata.
        file_path: Path to the input CSV file.

    Returns:
        An IngestionResult containing successful records and data quality issues.
    """
    dataframe = read_csv(file_path)

    successful_records = []
    quality_issues = []

    for index, row in enumerate(dataframe.to_dict(orient="records"), start=1):
        try:
            raw_row = _map_to_raw_transaction_row(row)
            normalized_record = normalize_transaction_row(raw_row, source=batch.source_name)

            validation_errors = validate_transaction(normalized_record)

            if validation_errors:
                for error in validation_errors:
                    quality_issues.append(
                        DataQualityIssue(
                            row_number=index,
                            raw_description=raw_row.description,
                            issue_type="validation_error",
                            message=error
                        )
                    )
                continue

            successful_records.append(normalized_record)

        except ValueError as ex:
            quality_issues.append(
                DataQualityIssue(
                    row_number=index,
                    raw_description=str(row.get("description", "")),
                    issue_type="normalization_error",
                    message=str(ex)
                )
            )

    return IngestionResult(
        successful_records=successful_records,
        quality_issues=quality_issues
    )


def _map_to_raw_transaction_row(row: dict) -> RawTransactionRow:
    """
    Maps a dictionary row to a RawTransactionRow model.
    """
    return RawTransactionRow(
        date=str(row.get("date", "")),
        description=str(row.get("description", "")),
        amount=str(row.get("amount", ""))
    )