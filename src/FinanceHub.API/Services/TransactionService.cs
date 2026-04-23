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

    public async Task<List<TransactionDto>> GetTransactionsAsync(string? category = null)
    {
        var results = new List<TransactionDto>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = """
            SELECT id, transaction_date, description, amount, currency, source, category
            FROM transactions
            """;

        if (!string.IsNullOrWhiteSpace(category))
        {
            query += """
                
                WHERE category = @category
                """;
        }

        query += """
            
            ORDER BY transaction_date DESC, id DESC;
            """;

        await using var command = new NpgsqlCommand(query, connection);

        if (!string.IsNullOrWhiteSpace(category))
        {
            command.Parameters.AddWithValue("category", category);
        }

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

    public async Task<TransactionSummaryDto> GetSummaryAsync(string? category = null)
    {
        decimal totalIncome = 0;
        decimal totalExpenses = 0;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = """
            SELECT amount
            FROM transactions
            """;

        if (!string.IsNullOrWhiteSpace(category))
        {
            query += """
                
                WHERE category = @category
                """;
        }

        query += ";";

        await using var command = new NpgsqlCommand(query, connection);

        if (!string.IsNullOrWhiteSpace(category))
        {
            command.Parameters.AddWithValue("category", category);
        }

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

    public async Task<List<CategorySummaryDto>> GetCategorySummariesAsync()
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
            GROUP BY COALESCE(NULLIF(category, ''), 'Uncategorized')
            ORDER BY category;
            """;

        await using var command = new NpgsqlCommand(query, connection);
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

    public async Task<List<TransactionTrendDto>> GetTransactionTrendsAsync()
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
            GROUP BY transaction_date
            ORDER BY transaction_date;
            """;

        await using var command = new NpgsqlCommand(query, connection);
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

}