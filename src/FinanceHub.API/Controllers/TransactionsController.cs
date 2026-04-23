using FinanceHub.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionsController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? category = null)
    {
        var transactions = await _transactionService.GetTransactionsAsync(category);
        return Ok(transactions);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] string? category = null)
    {
        var summary = await _transactionService.GetSummaryAsync(category);
        return Ok(summary);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _transactionService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("categories/summary")]
    public async Task<IActionResult> GetCategorySummaries()
    {
        var summaries = await _transactionService.GetCategorySummariesAsync();
        return Ok(summaries);
    }

    [HttpGet("trends")]
    public async Task<IActionResult> GetTrends()
    {
        var trends = await _transactionService.GetTransactionTrendsAsync();
        return Ok(trends);
    }
}