from financehub_ingestion.models.transaction_record import TransactionRecord


def validate_transaction(record: TransactionRecord) -> list[str]:
    """
    Validates a normalized transaction record.

    Args:
        record: The normalized transaction record.

    Returns:
        A list of validation error messages. An empty list means valid.
    """
    errors: list[str] = []

    if not record.description or not record.description.strip():
        errors.append("Description is required.")

    if record.currency != "EUR":
        errors.append("Only EUR currency is currently supported.")

    if abs(record.amount) > 1_000_000:
        errors.append("Amount exceeds allowed threshold.")

    return errors