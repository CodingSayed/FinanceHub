import pandas as pd


def read_csv(file_path: str) -> pd.DataFrame:
    """
    Reads a CSV file and returns a pandas DataFrame.

    Args:
        file_path: Path to the CSV file.

    Returns:
        A pandas DataFrame containing the CSV data.
    """
    try:
        return pd.read_csv(file_path)
    except Exception as ex:
        raise RuntimeError(f"Failed to read CSV: {file_path}") from ex