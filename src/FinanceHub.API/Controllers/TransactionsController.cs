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
    public async Task<IActionResult> Get(
        [FromQuery] string? category = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var transactions = await _transactionService.GetTransactionsAsync(
            category,
            startDate,
            endDate,
            page,
            pageSize);

        return Ok(transactions);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] string? category = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var summary = await _transactionService.GetSummaryAsync(category, startDate, endDate);
        return Ok(summary);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _transactionService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("categories/summary")]
    public async Task<IActionResult> GetCategorySummaries(
        [FromQuery] string? category = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var summaries = await _transactionService.GetCategorySummariesAsync(category, startDate, endDate);
        return Ok(summaries);
    }

    [HttpGet("trends")]
    public async Task<IActionResult> GetTrends(
        [FromQuery] string? category = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var trends = await _transactionService.GetTransactionTrendsAsync(category, startDate, endDate);
        return Ok(trends);
    }
}