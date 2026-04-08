from datetime import date
from decimal import Decimal
from pydantic import BaseModel


class TransactionRecord(BaseModel):
    """
    Represents a normalized finance transaction record used internally
    by the ingestion pipeline.
    """

    transaction_date: date
    description: str
    amount: Decimal
    currency: str
    source: str