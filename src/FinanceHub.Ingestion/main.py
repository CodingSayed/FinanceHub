from pathlib import Path
import sys

CURRENT_DIR = Path(__file__).resolve().parent
SRC_DIR = CURRENT_DIR / "src"

if str(SRC_DIR) not in sys.path:
    sys.path.insert(0, str(SRC_DIR))

from financehub_ingestion.models.data_quality_issue import DataQualityIssue
from financehub_ingestion.models.raw_transaction_row import RawTransactionRow
from financehub_ingestion.normalizers.transaction_normalizer import normalize_transaction_row
from financehub_ingestion.readers.csv_reader import read_csv
from financehub_ingestion.validators.transaction_validator import validate_transaction


def main() -> None:
    """
    Entry point for the FinanceHub ingestion application.
    """
    print("FinanceHub Ingestion started...")

    sample_file_path = CURRENT_DIR.parent.parent / "sample-data" / "sample.csv"
    dataframe = read_csv(str(sample_file_path))

    normalized_records = []
    quality_issues = []

    for index, row in enumerate(dataframe.to_dict(orient="records"), start=1):
        try:
            raw_row = _map_to_raw_transaction_row(row)
            normalized_record = normalize_transaction_row(raw_row, source="sample-csv")

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

            normalized_records.append(normalized_record)

        except ValueError as ex:
            quality_issues.append(
                DataQualityIssue(
                    row_number=index,
                    raw_description=str(row.get("description", "")),
                    issue_type="normalization_error",
                    message=str(ex)
                )
            )

    print(f"Successfully normalized {len(normalized_records)} transaction(s).")
    print(f"Detected {len(quality_issues)} data quality issue(s).")

    print("\nNormalized records:")
    for record in normalized_records:
        print(record.model_dump())

    print("\nData quality issues:")
    for issue in quality_issues:
        print(issue.model_dump())


def _map_to_raw_transaction_row(row: dict) -> RawTransactionRow:
    """
    Maps a dictionary row to a RawTransactionRow model.
    """
    return RawTransactionRow(
        date=str(row.get("date", "")),
        description=str(row.get("description", "")),
        amount=str(row.get("amount", ""))
    )


if __name__ == "__main__":
    main()