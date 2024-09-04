using Application.Interfaces;
using accountSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accountSystem.Application.Services
{
    public class LedgerService
    {
        private readonly ILedgerRepository _ledgerRepository;
        private readonly IAccountRepository _accountRepository;

        public LedgerService(ILedgerRepository ledgerRepository, IAccountRepository accountRepository)
        {
            _ledgerRepository = ledgerRepository;
            _accountRepository = accountRepository;
        }

        // جلب إدخالات دفتر الأستاذ مع الفلاتر
        public async Task<IEnumerable<LedgerEntry>> GetLedgerEntriesAsync(DateTime? startDate, DateTime? endDate, string accountType)
        {
            var entries = await _ledgerRepository.GetLedgerEntriesAsync(startDate, endDate, accountType);

            // تحقق من صحة إدخالات دفتر الأستاذ
            foreach (var entry in entries)
            {
                var account = await _accountRepository.GetAccountByIdAsync(entry.AccountId);
                if (account.CurrentBalance != entry.BalanceAfterTransaction)
                {
                    throw new InvalidOperationException("Inconsistency detected in ledger entry.");
                }
            }

            return entries;
        }

        // توليد ملخص دفتر الأستاذ
        public async Task<LedgerSummary> GetLedgerSummaryAsync(DateTime? startDate, DateTime? endDate, string accountType)
        {
            var entries = await _ledgerRepository.GetLedgerEntriesAsync(startDate, endDate, accountType);

            var totalDebits = entries.Where(e => e.IsDebit).Sum(e => e.Amount);
            var totalCredits = entries.Where(e => !e.IsDebit).Sum(e => e.Amount);
            //var accountBalances = entries.GroupBy(e => e.AccountId)
            //                             .Select(g => new AccountBalanceInfo
            //                             {
            //                                 AccountId = g.Key,
            //                                 Balance = g.Sum(e => e.IsDebit ? -e.Amount : e.Amount)
            //                             }).ToList();

            return new LedgerSummary
            {
                TotalDebits = totalDebits,
                TotalCredits = totalCredits,
                Balance = totalCredits - totalDebits,
            };
        }




    }

    public class LedgerSummary
    {
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal Balance { get; set; }
    }

}
