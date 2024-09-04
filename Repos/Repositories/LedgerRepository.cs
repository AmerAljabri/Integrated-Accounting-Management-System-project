using Application.Interfaces;
using accountSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LedgerRepository : ILedgerRepository
    {
        private readonly ApplicationDbContext _context;

        public LedgerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LedgerEntry>> GetLedgerEntriesAsync(DateTime? startDate, DateTime? endDate, string accountType)
        {
            var query = _context.LedgerEntries.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.CreatedAt <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(accountType))
            {
                query = query.Where(e => e.Account.AccountType == accountType);
            }

            return await query.ToListAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
    }
}
