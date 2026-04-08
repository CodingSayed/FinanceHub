from datetime import datetime
from pydantic import BaseModel


class ImportBatch(BaseModel):
    """
    Represents a single ingestion batch execution.
    """

    source_name: str
    file_name: str
    started_at_utc: datetime