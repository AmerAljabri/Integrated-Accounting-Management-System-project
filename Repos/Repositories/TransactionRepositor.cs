using accountSystem.Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasTransactionsForAccountAsync(int accountId)
        {
            return await _context.TransactionAccounts.AnyAsync(ta => ta.AccountId == accountId);
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.Include(t => t.TransactionAccounts).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime? startDate, DateTime? endDate, string accountType, decimal? amount)
        {
            var query = _context.Transactions.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(t => t.TransactionDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.TransactionDate <= endDate.Value);

            if (!string.IsNullOrEmpty(accountType))
                query = query.Where(t => t.TransactionAccounts.Any(ta => ta.Account.AccountType == accountType));

            if (amount.HasValue)
                query = query.Where(t => t.TransactionAccounts.Any(ta => ta.Amount == amount.Value));

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                                 .Where(t => t.TransactionAccounts.Any(ta => ta.AccountId == accountId))
                                 .Include(t => t.TransactionAccounts) // تضمين `TransactionAccounts` للحصول على التفاصيل المرتبطة
                                 .ToListAsync();
        }
        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.Transactions
                                 .Include(t => t.TransactionAccounts) // تضمين `TransactionAccounts` للحصول على التفاصيل المرتبطة
                                 .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }



        public async Task DeleteTransactionAsync(int transactionId)
        {
            var transaction = await GetTransactionByIdAsync(transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
