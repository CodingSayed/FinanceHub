from pathlib import Path
import sys

CURRENT_DIR = Path(__file__).resolve().parent
SRC_DIR = CURRENT_DIR / "src"

if str(SRC_DIR) not in sys.path:
    sys.path.insert(0, str(SRC_DIR))

from financehub_ingestion.readers.csv_reader import read_csv


def main() -> None:
    """
    Entry point for the FinanceHub ingestion application.
    """
    print("FinanceHub Ingestion started...")

    sample_file_path = CURRENT_DIR.parent.parent / "sample-data" / "sample.csv"
    data = read_csv(str(sample_file_path))

    print(data.head())


if __name__ == "__main__":
    main()