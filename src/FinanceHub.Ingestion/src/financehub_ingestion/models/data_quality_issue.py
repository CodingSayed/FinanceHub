from pydantic import BaseModel


class DataQualityIssue(BaseModel):
    """
    Represents a data quality issue encountered during ingestion.
    """

    row_number: int
    raw_description: str
    issue_type: str
    message: str