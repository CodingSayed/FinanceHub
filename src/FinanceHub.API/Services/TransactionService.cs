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
            SELECT id, transaction_date, description, amount, currency, source
            FROM transactions
            ORDER BY transaction_date DESC;
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
                Source = reader.GetString(5)
            });
        }

        return results;
    }
}