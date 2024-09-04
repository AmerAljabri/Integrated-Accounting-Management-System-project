using accountSystem.Application.Services; // Ensure this namespace is included
using accountSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace accountSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AccountService _accountService; // Add AccountService
        private readonly TransactionService _transactionService; // Add TransactionService
        private readonly AccountBalanceService _accountBalanceService;

        public HomeController(ILogger<HomeController> logger, AccountService accountService, TransactionService transactionService, AccountBalanceService accountBalanceService)
        {
            _logger = logger;
            _accountService = accountService; // Initialize AccountService
            _transactionService = transactionService; // Initialize TransactionService
            _accountBalanceService = accountBalanceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard() // Add Dashboard action
        {
            var accountBalances = await  _accountBalanceService.GetAccountBalancesAsync();
            var transactionSummary = await _transactionService.GetTransactionSummaryAsync();

            ViewBag.AccountBalances = accountBalances; // Example: [1200, 1500, 800]
            ViewBag.TransactionSummary = transactionSummary; // Example: [3000, 1500, 1000]

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}