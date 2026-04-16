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

    public async Task<List<TransactionDto>> GetTransactionsAsync()
    {
        var results = new List<TransactionDto>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            SELECT id, transaction_date, description, amount, currency, source, category
            FROM transactions
        ";

        await using var command = new NpgsqlCommand(query, connection);
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
                Category = reader.GetString(6)
            });
        }

        return results;
    }

    public async Task<TransactionSummaryDto> GetSummaryAsync()
    {
        decimal totalIncome = 0;
        decimal totalExpenses = 0;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            SELECT amount
            FROM transactions;
        ";

        await using var command = new NpgsqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var amount = reader.GetDecimal(0);

            if (amount > 0)
                totalIncome += amount;
            else
                totalExpenses += amount;
        }

        return new TransactionSummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            NetBalance = totalIncome + totalExpenses
        };
    }
}