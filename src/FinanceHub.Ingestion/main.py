from pathlib import Path
import sys

import pandas as pd

CURRENT_DIR = Path(__file__).resolve().parent
SRC_DIR = CURRENT_DIR / "src"

if str(SRC_DIR) not in sys.path:
    sys.path.insert(0, str(SRC_DIR))

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

    for row in dataframe.to_dict(orient="records"):
        raw_row = _map_to_raw_transaction_row(row)
        normalized_record = normalize_transaction_row(raw_row, source="sample-csv")

        validation_errors = validate_transaction(normalized_record)

        if validation_errors:
            print(f"Validation errors for row '{raw_row.description}':")
            for error in validation_errors:
                print(f" - {error}")
            continue

        normalized_records.append(normalized_record)

    print(f"Successfully normalized {len(normalized_records)} transaction(s).")

    for record in normalized_records:
        print(record.model_dump())


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