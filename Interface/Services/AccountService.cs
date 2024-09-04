using accountSystem.Domain.Entities;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;

namespace accountSystem.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<AccountDto> GetAccountByIdAsync(int accountId)
        {
            return await _accountRepository.GetAccountByIdAsync(accountId);
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync(); // Fetch Account entities
            return accounts.Select(account => new AccountDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                OpeningBalance = account.OpeningBalance,
                CurrentBalance = account.CurrentBalance,
                IsActive = account.IsActive,
                AccountType = account.AccountType,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            }).ToList();
        }
        public async Task<IEnumerable<Transaction>> GetAccountActivitiesAsync(int accountId)
        {
            return await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);
        }



        public async Task ActivateAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            account.IsActive = true;
            account.UpdatedAt = DateTime.Now;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task DeactivateAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            account.IsActive = false;
            account.UpdatedAt = DateTime.Now;

            await _accountRepository.UpdateAccountAsync(account);
        }

          
            public async Task<bool> HasTransactionsForAccountAsync(int accountId)
            {
                return await _transactionRepository.HasTransactionsForAccountAsync(accountId);
            }

        // بقية وظائف الخدمة






        public async Task CreateAccountAsync(AccountDto account)
        {
            // التحقق من تفاصيل الحساب المدخلة
            if (string.IsNullOrEmpty(account.AccountNumber) || account.OpeningBalance < 0)
            {
                throw new InvalidOperationException("Invalid account details.");
            }

            // تعيين التاريخ الحالي
            account.CreatedAt = DateTime.Now;
            account.UpdatedAt = DateTime.Now;

            // تعيين CurrentBalance إلى نفس قيمة OpeningBalance عند إنشاء الحساب
            account.CurrentBalance = account.OpeningBalance;

            // إضافة الحساب إلى قاعدة البيانات
            await _accountRepository.AddAccountAsync(account);
        
        }


        public async Task UpdateAccountAsync(AccountDto accountDto)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountDto.AccountId);
            if (account != null)
            {
                // قم بتحديث البيانات
                _mapper.Map(accountDto, account);  // تحديث القيم باستخدام AutoMapper
                await _accountRepository.UpdateAccountAsync(account);  // حفظ التغييرات في قاعدة البيانات
            }
        }


        public async Task DeleteAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            // تحقق من المعاملات المرتبطة
            var hasTransactions = await _transactionRepository.HasTransactionsForAccountAsync(accountId);
            if (hasTransactions)
            {
                throw new InvalidOperationException("Cannot delete an account with associated transactions.");
            }

            await _accountRepository.DeleteAccountAsync(accountId);
        }
       
    }
}