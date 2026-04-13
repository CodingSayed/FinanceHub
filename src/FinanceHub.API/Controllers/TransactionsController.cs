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
    public async Task<IActionResult> Get()
    {
        var transactions = await _transactionService.GetTransactionsAsync();
        return Ok(transactions);
    }
}