from datetime import datetime
from decimal import Decimal, InvalidOperation

from financehub_ingestion.models.raw_transaction_row import RawTransactionRow
from financehub_ingestion.models.transaction_record import TransactionRecord


def normalize_transaction_row(raw_row: RawTransactionRow, source: str) -> TransactionRecord:
    """
    Converts a raw transaction row into a normalized TransactionRecord.

    Args:
        raw_row: The raw transaction row from a CSV file.
        source: The logical source name of the imported file.

    Returns:
        A normalized TransactionRecord.

    Raises:
        ValueError: When date or amount cannot be parsed.
    """
    transaction_date = _parse_date(raw_row.date)
    amount = _parse_amount(raw_row.amount)

    return TransactionRecord(
        transaction_date=transaction_date,
        description=raw_row.description.strip(),
        amount=amount,
        currency="EUR",
        source=source
    )


def _parse_date(value: str):
    """
    Parses a transaction date in ISO format (YYYY-MM-DD).
    """
    try:
        return datetime.strptime(value.strip(), "%Y-%m-%d").date()
    except ValueError as ex:
        raise ValueError(f"Invalid transaction date: '{value}'") from ex


def _parse_amount(value: str) -> Decimal:
    """
    Parses a decimal amount from a string.
    """
    try:
        normalized_value = value.strip().replace(",", ".")
        return Decimal(normalized_value)
    except (InvalidOperation, AttributeError) as ex:
        raise ValueError(f"Invalid transaction amount: '{value}'") from ex