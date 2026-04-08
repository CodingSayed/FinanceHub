from pydantic import BaseModel


class RawTransactionRow(BaseModel):
    """
    Represents a raw transaction row as it is read from an input CSV file.
    """

    date: str
    description: str
    amount: str