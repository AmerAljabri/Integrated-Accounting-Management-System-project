using accountSystem.Domain.Entities;
using Application.Interfaces;

namespace accountSystem.Application.Services
{
    public class AccountBalanceService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountBalanceService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // جلب أرصدة الحسابات
        public async Task<IEnumerable<AccountBalanceDto>> GetAccountBalancesAsync()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();
            var accountBalances = accounts.Select(a => new AccountBalanceDto
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType,
                CurrentBalance = a.CurrentBalance
            }).ToList();

            return accountBalances;
        }
    }

    public class AccountBalanceDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
       
    }
}
