namespace FinanceHub.API.Models;

public class TransactionTrendDto
{
    public DateTime Date { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal NetBalance { get; set; }
}