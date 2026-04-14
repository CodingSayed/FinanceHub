namespace FinanceHub.Ui.Models.Transactions;

public class TransactionViewModel
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}