from datetime import datetime, date
from decimal import Decimal, InvalidOperation

from financehub_ingestion.models.raw_transaction_row import RawTransactionRow
from financehub_ingestion.models.transaction_record import TransactionRecord


SUPPORTED_DATE_FORMATS = [
    "%Y-%m-%d",
    "%d-%m-%Y",
    "%d/%m/%Y",
]

SUPPORTED_CURRENCY = "EUR"


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
        currency=SUPPORTED_CURRENCY,
        source=source
    )


def _parse_date(value: str) -> date:
    """
    Parses a transaction date using a set of supported input formats.
    """
    cleaned_value = value.strip()

    for date_format in SUPPORTED_DATE_FORMATS:
        try:
            return datetime.strptime(cleaned_value, date_format).date()
        except ValueError:
            continue

    raise ValueError(
        f"Invalid transaction date: '{value}'. "
        f"Supported formats: {', '.join(SUPPORTED_DATE_FORMATS)}"
    )


def _parse_amount(value: str) -> Decimal:
    """
    Parses a decimal amount from a string.

    Supported examples:
    - 1234.56
    - 1234,56
    - -45.20
    - -45,20
    """
    try:
        cleaned_value = value.strip().replace(" ", "")

        if "," in cleaned_value and "." in cleaned_value:
            cleaned_value = cleaned_value.replace(".", "").replace(",", ".")
        else:
            cleaned_value = cleaned_value.replace(",", ".")

        return Decimal(cleaned_value)
    except (InvalidOperation, AttributeError) as ex:
        raise ValueError(f"Invalid transaction amount: '{value}'") from ex