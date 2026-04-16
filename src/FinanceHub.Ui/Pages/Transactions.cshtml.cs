using System.Text.Json;
using FinanceHub.Ui.Models.Transactions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinanceHub.Ui.Pages;

public class TransactionsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    public TransactionSummaryViewModel? Summary { get; private set; }

    public TransactionsModel(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public List<TransactionViewModel> Transactions { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var baseUrl = _configuration["ApiSettings:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return;
        }

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{baseUrl}/api/transactions");

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        Transactions = JsonSerializer.Deserialize<List<TransactionViewModel>>(json, options) ?? [];

        var summaryResponse = await client.GetAsync($"{baseUrl}/api/transactions/summary");

        if (summaryResponse.IsSuccessStatusCode)
        {
            var summaryJson = await summaryResponse.Content.ReadAsStringAsync();

            Summary = JsonSerializer.Deserialize<TransactionSummaryViewModel>(
                summaryJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
    }
}