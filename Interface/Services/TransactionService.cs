using accountSystem.Domain.Entities;
using Application.Interfaces;

namespace accountSystem.Application.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        // Record a transaction
        public async Task RecordTransactionAsync(string transactionType, DateTime transactionDate, List<TransactionAccount> transactionAccounts)
        {
            // التحقق من الحسابات المرتبطة
            foreach (var transactionAccount in transactionAccounts)
            {
                var account = await _accountRepository.GetAccountByIdAsync(transactionAccount.AccountId);
                if (account == null)
                {
                    throw new InvalidOperationException($"Account with ID {transactionAccount.AccountId} does not exist.");
                }
            }

            var totalDebits = transactionAccounts.Where(ta => ta.IsDebit).Sum(ta => ta.Amount);
            var totalCredits = transactionAccounts.Where(ta => !ta.IsDebit).Sum(ta => ta.Amount);

            if (totalDebits != totalCredits)
            {
                throw new InvalidOperationException("Total debits must equal total credits.");
            }

            var transaction = new Transaction
            {
                TransactionType = transactionType,
                TransactionDate = transactionDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                TransactionAccounts = transactionAccounts
            };

            foreach (var transactionAccount in transactionAccounts)
            {
                var account = await _accountRepository.GetAccountByIdAsync(transactionAccount.AccountId);
                if (account == null) throw new Exception("Account not found");

                account.CurrentBalance += transactionAccount.IsDebit ? -transactionAccount.Amount : transactionAccount.Amount;
                account.UpdatedAt = DateTime.Now;

                await _accountRepository.UpdateAccountAsync(account);
            }

            await _transactionRepository.AddTransactionAsync(transaction);
        }

        // Get transactions with filters
        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(DateTime? startDate, DateTime? endDate, string accountType, decimal? amount)
        {
            return await _transactionRepository.GetTransactionsAsync(startDate, endDate, accountType, amount);
        }

        // Get transaction by ID
        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _transactionRepository.GetTransactionByIdAsync(transactionId);
        }

        // Update transaction
        public async Task UpdateTransactionAsync(int transactionId, string transactionType, DateTime transactionDate, List<TransactionAccount> transactionAccounts)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
            if (transaction == null) throw new InvalidOperationException("Transaction not found.");
            
            if (IsPostedToReports(transactionId))
            {
                throw new InvalidOperationException("Cannot modify a transaction that has been posted to financial reports.");
            }

            var totalDebits = transactionAccounts.Where(ta => ta.IsDebit).Sum(ta => ta.Amount);
            var totalCredits = transactionAccounts.Where(ta => !ta.IsDebit).Sum(ta => ta.Amount);

            if (totalDebits != totalCredits)
            {
                throw new InvalidOperationException("Total debits must equal total credits.");
            }

            transaction.TransactionType = transactionType;
            transaction.TransactionDate = transactionDate;
            transaction.UpdatedAt = DateTime.Now;
            transaction.TransactionAccounts = transactionAccounts;

            foreach (var transactionAccount in transactionAccounts)
            {
                var account = await _accountRepository.GetAccountByIdAsync(transactionAccount.AccountId);
                if (account == null) throw new Exception("Account not found");

                account.CurrentBalance += transactionAccount.IsDebit ? -transactionAccount.Amount : transactionAccount.Amount;
                account.UpdatedAt = DateTime.Now;

                await _accountRepository.UpdateAccountAsync(account);
            }

            await _transactionRepository.AddTransactionAsync(transaction);
        }

        // Delete transaction
        public async Task DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
            if (transaction == null) throw new InvalidOperationException("Transaction not found.");

            if (IsPostedToReports(transactionId))
            {
                throw new InvalidOperationException("Cannot delete a transaction that has been posted to financial reports.");
            }

            await _transactionRepository.DeleteTransactionAsync(transactionId);
        }
        public async Task<List<decimal>> GetTransactionSummaryAsync()
        {
            // Fetch transactions and summarize them
            var transactions = await _transactionRepository.GetAllTransactionsAsync(); // Assuming this method exists

            return transactions
                .GroupBy(t => t.TransactionType) // Group by transaction type
                .Select(g => g.Sum(t => t.TransactionAccounts.Sum(ta => ta.IsDebit ? -ta.Amount : ta.Amount))) // Summarize amounts
                .ToList(); // Convert to a list
        }

        private bool IsPostedToReports(int transactionId)
        {
            // Logic to check if the transaction has been posted to reports or financial statements
            return false; // Example logic
        }
    }
}
