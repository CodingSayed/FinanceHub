namespace FinanceHub.Ui.Models.Transactions;

public class TransactionTrendViewModel
{
    public DateTime Date { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal NetBalance { get; set; }
}