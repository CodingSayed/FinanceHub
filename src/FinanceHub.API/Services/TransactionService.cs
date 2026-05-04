using FinanceHub.API.Models;
using Npgsql;

namespace FinanceHub.API.Services;

public class TransactionService
{
    private readonly string _connectionString;

    public TransactionService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")!;
    }

    public async Task<List<TransactionDto>> GetTransactionsAsync(
        string? category = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var results = new List<TransactionDto>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = """
            SELECT id, transaction_date, description, amount, currency, source, category
            FROM transactions
            """;

        var conditions = new List<string>();

        if (!string.IsNullOrWhiteSpace(category))
        {
            conditions.Add("category = @category");
        }

        if (startDate.HasValue)
        {
            conditions.Add("transaction_date >= @startDate");
        }

        if (endDate.HasValue)
        {
            conditions.Add("transaction_date <= @endDate");
        }

        if (conditions.Count > 0)
        {
            query += $" WHERE {string.Join(" AND ", conditions)}";
        }

        query += """
            
            ORDER BY transaction_date DESC, id DESC;
            """;

        await using var command = new NpgsqlCommand(query, connection);
        AddFilterParameters(command, category, startDate, endDate);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new TransactionDto
            {
                Id = reader.GetInt32(0),
                TransactionDate = reader.GetDateTime(1),
                Description = reader.GetString(2),
                Amount = reader.GetDecimal(3),
                Currency = reader.GetString(4),
                Source = reader.GetString(5),
                Category = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
            });
        }

        return results;
    }

    public async Task<TransactionSummaryDto> GetSummaryAsync(
        string? category = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        decimal totalIncome = 0;
        decimal totalExpenses = 0;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = """
            SELECT amount
            FROM transactions
            """;

        var conditions = BuildFilterConditions(category, startDate, endDate);

        if (conditions.Count > 0)
        {
            query += $" WHERE {string.Join(" AND ", conditions)}";
        }

        query += ";";

        await using var command = new NpgsqlCommand(query, connection);
        AddFilterParameters(command, category, startDate, endDate);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var amount = reader.GetDecimal(0);

            if (amount > 0)
            {
                totalIncome += amount;
            }
            else
            {
                totalExpenses += amount;
            }
        }

        return new TransactionSummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            NetBalance = totalIncome + totalExpenses
        };
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        var results = new List<string>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = """
            SELECT DISTINCT category
            FROM transactions
            WHERE category IS NOT NULL
              AND category <> ''
            ORDER BY category;
            """;

        await using var command = new NpgsqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(reader.GetString(0));
        }

        return results;
    }

    public async Task<List<CategorySummaryDto>> GetCategorySummariesAsync(
        string? category = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var results = new List<CategorySummaryDto>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = """
            SELECT
                COALESCE(NULLIF(category, ''), 'Uncategorized') AS category,
                COALESCE(SUM(CASE WHEN amount > 0 THEN amount ELSE 0 END), 0) AS total_income,
                COALESCE(SUM(CASE WHEN amount < 0 THEN amount ELSE 0 END), 0) AS total_expense,
                COALESCE(SUM(amount), 0) AS net_balance,
                COUNT(*) AS transaction_count
            FROM transactions
            """;

        var conditions = BuildFilterConditions(category, startDate, endDate);

        if (conditions.Count > 0)
        {
            query += $" WHERE {string.Join(" AND ", conditions)}";
        }

        query += """
            
            GROUP BY COALESCE(NULLIF(category, ''), 'Uncategorized')
            ORDER BY category;
            """;

        await using var command = new NpgsqlCommand(query, connection);
        AddFilterParameters(command, category, startDate, endDate);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new CategorySummaryDto
            {
                Category = reader.GetString(0),
                TotalIncome = reader.GetDecimal(1),
                TotalExpense = reader.GetDecimal(2),
                NetBalance = reader.GetDecimal(3),
                TransactionCount = reader.GetInt32(4)
            });
        }

        return results;
    }

    public async Task<List<TransactionTrendDto>> GetTransactionTrendsAsync(
        string? category = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var results = new List<TransactionTrendDto>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = """
            SELECT
                transaction_date,
                COALESCE(SUM(CASE WHEN amount > 0 THEN amount ELSE 0 END), 0) AS income,
                COALESCE(SUM(CASE WHEN amount < 0 THEN amount ELSE 0 END), 0) AS expense,
                COALESCE(SUM(amount), 0) AS net_balance
            FROM transactions
            """;

        var conditions = BuildFilterConditions(category, startDate, endDate);

        if (conditions.Count > 0)
        {
            query += $" WHERE {string.Join(" AND ", conditions)}";
        }

        query += """
            
            GROUP BY transaction_date
            ORDER BY transaction_date;
            """;

        await using var command = new NpgsqlCommand(query, connection);
        AddFilterParameters(command, category, startDate, endDate);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new TransactionTrendDto
            {
                Date = reader.GetDateTime(0),
                Income = reader.GetDecimal(1),
                Expense = reader.GetDecimal(2),
                NetBalance = reader.GetDecimal(3)
            });
        }

        return results;
    }

    private static List<string> BuildFilterConditions(
        string? category,
        DateTime? startDate,
        DateTime? endDate)
    {
        var conditions = new List<string>();

        if (!string.IsNullOrWhiteSpace(category))
        {
            conditions.Add("category = @category");
        }

        if (startDate.HasValue)
        {
            conditions.Add("transaction_date >= @startDate");
        }

        if (endDate.HasValue)
        {
            conditions.Add("transaction_date <= @endDate");
        }

        return conditions;
    }

    private static void AddFilterParameters(
        NpgsqlCommand command,
        string? category,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!string.IsNullOrWhiteSpace(category))
        {
            command.Parameters.AddWithValue("category", category);
        }

        if (startDate.HasValue)
        {
            command.Parameters.AddWithValue("startDate", startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            command.Parameters.AddWithValue("endDate", endDate.Value.Date);
        }
    }
}