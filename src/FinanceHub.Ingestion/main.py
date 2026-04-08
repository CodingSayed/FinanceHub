from datetime import datetime, UTC
from pathlib import Path
import sys

CURRENT_DIR = Path(__file__).resolve().parent
SRC_DIR = CURRENT_DIR / "src"

if str(SRC_DIR) not in sys.path:
    sys.path.insert(0, str(SRC_DIR))

from financehub_ingestion.models.import_batch import ImportBatch
from financehub_ingestion.services.ingestion_pipeline import run_ingestion


def main() -> None:
    """
    Entry point for the FinanceHub ingestion application.
    """
    print("FinanceHub Ingestion started...")

    sample_file_path = CURRENT_DIR.parent.parent / "sample-data" / "sample.csv"

    batch = ImportBatch(
        source_name="sample-csv",
        file_name=sample_file_path.name,
        started_at_utc=datetime.now(UTC)
    )

    result = run_ingestion(batch=batch, file_path=str(sample_file_path))

    print(f"Import source: {batch.source_name}")
    print(f"File name: {batch.file_name}")
    print(f"Started at (UTC): {batch.started_at_utc.isoformat()}")
    print(f"Successfully normalized {result.successful_count} transaction(s).")
    print(f"Detected {result.quality_issue_count} data quality issue(s).")

    print("\nNormalized records:")
    for record in result.successful_records:
        print(record.model_dump())

    print("\nData quality issues:")
    for issue in result.quality_issues:
        print(issue.model_dump())


if __name__ == "__main__":
    main()