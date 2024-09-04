using accountSystem.Domain.Entities;

namespace Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);

        Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime? startDate, DateTime? endDate, string accountType, decimal? amount);
        Task<Transaction> GetTransactionByIdAsync(int transactionId);
        Task<bool> HasTransactionsForAccountAsync(int accountId);
        Task DeleteTransactionAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    }
}
