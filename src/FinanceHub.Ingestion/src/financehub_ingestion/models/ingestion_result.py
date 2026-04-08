from pydantic import BaseModel

from financehub_ingestion.models.data_quality_issue import DataQualityIssue
from financehub_ingestion.models.transaction_record import TransactionRecord


class IngestionResult(BaseModel):
    """
    Represents the result of a single ingestion run.
    """

    successful_records: list[TransactionRecord]
    quality_issues: list[DataQualityIssue]

    @property
    def successful_count(self) -> int:
        return len(self.successful_records)

    @property
    def quality_issue_count(self) -> int:
        return len(self.quality_issues)