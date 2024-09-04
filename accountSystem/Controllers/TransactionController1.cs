using accountSystem.Application.Services;
using accountSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace accountSystem.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly AccountService _accountService;

        public TransactionController(TransactionService transactionService, AccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }

        // View all transactions with optional filters
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string accountType, decimal? amount)
        {
            var transactions = await _transactionService.GetTransactionsAsync(startDate, endDate, accountType, amount);
            return View(transactions);
        }

        // GET: Create transaction form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Accounts = await _accountService.GetAllAccountsAsync(); // Pass the list of accounts to the view
            return View();
        }

        // POST: Create a new transaction
        [HttpPost]
        public async Task<IActionResult> Create(string transactionType, DateTime transactionDate, List<TransactionAccount> transactionAccounts)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Accounts = await _accountService.GetAllAccountsAsync(); // Reload accounts on validation failure
                return View(transactionAccounts);
            }

            try
            {
                await _transactionService.RecordTransactionAsync(transactionType, transactionDate, transactionAccounts);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Accounts = await _accountService.GetAllAccountsAsync(); // Reload accounts on validation failure
                return View(transactionAccounts);
            }
        }

        // GET: Transaction Details for editing
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            ViewBag.Accounts = await _accountService.GetAllAccountsAsync(); // Pass accounts list to the view for editing
            return View(transaction);
        }

        // POST: Update an existing transaction
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string transactionType, DateTime transactionDate, List<TransactionAccount> transactionAccounts)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Accounts = await _accountService.GetAllAccountsAsync(); // Reload accounts on validation failure
                return View(transactionAccounts);
            }

            try
            {
                await _transactionService.UpdateTransactionAsync(id, transactionType, transactionDate, transactionAccounts);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Accounts = await _accountService.GetAllAccountsAsync(); // Reload accounts on validation failure
                return View(transactionAccounts);
            }
        }

        // POST: Delete a transaction
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", await _transactionService.GetTransactionsAsync(null, null, null, null)); // Return to index with error message
            }
        }
    }
}
