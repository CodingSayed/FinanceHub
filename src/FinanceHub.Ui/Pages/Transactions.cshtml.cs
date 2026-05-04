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

    [BindProperty(SupportsGet = true)]
    public DateTime? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? EndDate { get; set; }

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

        var filterQueryString = BuildFilterQueryString();

        var transactionsUrl = $"{baseUrl}/api/transactions{filterQueryString}";
        var summaryUrl = $"{baseUrl}/api/transactions/summary{filterQueryString}";
        var categoriesUrl = $"{baseUrl}/api/transactions/categories";
        var categorySummariesUrl = $"{baseUrl}/api/transactions/categories/summary{filterQueryString}";
        var trendsUrl = $"{baseUrl}/api/transactions/trends{filterQueryString}";

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

    private string BuildFilterQueryString()
    {
        var filters = new List<string>();

        if (!string.IsNullOrWhiteSpace(Category))
        {
            filters.Add($"category={Uri.EscapeDataString(Category)}");
        }

        if (StartDate.HasValue)
        {
            filters.Add($"startDate={StartDate.Value:yyyy-MM-dd}");
        }

        if (EndDate.HasValue)
        {
            filters.Add($"endDate={EndDate.Value:yyyy-MM-dd}");
        }

        if (filters.Count == 0)
        {
            return string.Empty;
        }

        return $"?{string.Join("&", filters)}";
    }
}