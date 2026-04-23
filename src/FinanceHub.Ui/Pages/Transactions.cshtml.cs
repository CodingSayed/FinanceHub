using System.Text.Json;
using FinanceHub.Ui.Models.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinanceHub.Ui.Pages;

public class TransactionsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public TransactionsModel(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public List<TransactionViewModel> Transactions { get; private set; } = [];
    public TransactionSummaryViewModel? Summary { get; private set; }
    public List<string> Categories { get; private set; } = [];
    public List<CategorySummaryViewModel> CategorySummaries { get; private set; } = [];
    public List<TransactionTrendViewModel> Trends { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    public async Task OnGetAsync()
    {
        var baseUrl = _configuration["ApiSettings:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return;
        }

        var client = _httpClientFactory.CreateClient();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var transactionsUrl = string.IsNullOrWhiteSpace(Category)
            ? $"{baseUrl}/api/transactions"
            : $"{baseUrl}/api/transactions?category={Uri.EscapeDataString(Category)}";

        var summaryUrl = string.IsNullOrWhiteSpace(Category)
            ? $"{baseUrl}/api/transactions/summary"
            : $"{baseUrl}/api/transactions/summary?category={Uri.EscapeDataString(Category)}";

        var categoriesUrl = $"{baseUrl}/api/transactions/categories";
        var categorySummariesUrl = $"{baseUrl}/api/transactions/categories/summary";
        var trendsUrl = $"{baseUrl}/api/transactions/trends";

        var transactionsResponse = await client.GetAsync(transactionsUrl);

        if (transactionsResponse.IsSuccessStatusCode)
        {
            var transactionsJson = await transactionsResponse.Content.ReadAsStringAsync();
            Transactions = JsonSerializer.Deserialize<List<TransactionViewModel>>(transactionsJson, options) ?? [];
        }

        var summaryResponse = await client.GetAsync(summaryUrl);

        if (summaryResponse.IsSuccessStatusCode)
        {
            var summaryJson = await summaryResponse.Content.ReadAsStringAsync();
            Summary = JsonSerializer.Deserialize<TransactionSummaryViewModel>(summaryJson, options);
        }

        var categoriesResponse = await client.GetAsync(categoriesUrl);

        if (categoriesResponse.IsSuccessStatusCode)
        {
            var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
            Categories = JsonSerializer.Deserialize<List<string>>(categoriesJson, options) ?? [];
        }

        var categorySummariesResponse = await client.GetAsync(categorySummariesUrl);

        if (categorySummariesResponse.IsSuccessStatusCode)
        {
            var categorySummariesJson = await categorySummariesResponse.Content.ReadAsStringAsync();
            CategorySummaries = JsonSerializer.Deserialize<List<CategorySummaryViewModel>>(categorySummariesJson, options) ?? [];
        }

        var trendsResponse = await client.GetAsync(trendsUrl);

        if (trendsResponse.IsSuccessStatusCode)
        {
            var trendsJson = await trendsResponse.Content.ReadAsStringAsync();
            Trends = JsonSerializer.Deserialize<List<TransactionTrendViewModel>>(trendsJson, options) ?? [];
        }
    }
}