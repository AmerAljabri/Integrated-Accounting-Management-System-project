using accountSystem.Domain.Entities;

namespace Application.Interfaces
{
    public interface ILedgerRepository
    {
        Task<IEnumerable<LedgerEntry>> GetLedgerEntriesAsync(DateTime? startDate, DateTime? endDate, string accountType);
        // إضافة هذه الدالة
        Task<Account> GetAccountByIdAsync(int accountId);
    }
}
