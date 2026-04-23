namespace FinanceHub.API.Models;

public sealed class CategorySummaryDto
{
    public string Category { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetBalance { get; set; }
    public int TransactionCount { get; set; }
}