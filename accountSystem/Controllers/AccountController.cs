using accountSystem.Application.Services;
using accountSystem.Domain.Entities;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace accountSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly AccountBalanceService _accountBalanceService;

        public AccountController(AccountService accountService, AccountBalanceService accountBalanceService)
        {
            _accountService = accountService;
            _accountBalanceService = accountBalanceService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }
        public async Task<IActionResult> Balances()
        {
            var accountBalances = await _accountBalanceService.GetAccountBalancesAsync();
            return View(accountBalances);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> AccountActivities(int accountId)
        {
            var activities = await _accountService.GetAccountActivitiesAsync(accountId);
            return View(activities);
        }


        [HttpPost]
        public async Task<IActionResult> Create(AccountDto account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            try
            {
                await _accountService.CreateAccountAsync(account);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(account);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AccountDto account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            try
            {
                await _accountService.UpdateAccountAsync(account);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(account);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(AccountDto account)
        {
            try
            {
                // استدعاء الخدمة لتحديث الحساب
                await _accountService.UpdateAccountAsync(account);
                return RedirectToAction(nameof(Index)); // الرجوع إلى الصفحة الرئيسية بعد النجاح
            }
            catch (Exception ex)
            {
                // عرض رسالة الخطأ
                ModelState.AddModelError("", ex.Message);
                return View("Index", await _accountService.GetAllAccountsAsync()); // عرض الحسابات الحالية مع رسالة الخطأ
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int accountId)
        {
            try
            {
                // التحقق إذا كان الحساب يحتوي على معاملات
                bool hasTransactions = await _accountService.HasTransactionsForAccountAsync(accountId);
                if (hasTransactions)
                {
                    // عرض رسالة الخطأ إذا كانت هناك معاملات مرتبطة
                    return Json(new { success = false, message = "Cannot delete account with associated transactions." });
                }             
                
              

                // إذا لم تكن هناك معاملات، يتم حذف الحساب
                await _accountService.DeleteAccountAsync(accountId);
                return Json(new { success = true }); // الرجوع إلى الصفحة الرئيسية بعد الحذف الناجح
            }
            catch (InvalidOperationException ex)
            {
                // عرض رسالة الخطأ في حال وجود استثناء
                ModelState.AddModelError("", ex.Message);
                return View("Index", await _accountService.GetAllAccountsAsync()); // عرض الحسابات الحالية مع رسالة الخطأ
            }
        }


        [HttpPost]
        public async Task<IActionResult> Activate(int accountId)
        {
            try
            {
                await _accountService.ActivateAccountAsync(accountId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", await _accountService.GetAllAccountsAsync()); // عرض رسالة الخطأ
            }
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(int accountId)
        {
            try
            {
                await _accountService.DeactivateAccountAsync(accountId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", await _accountService.GetAllAccountsAsync()); // عرض رسالة الخطأ
            }
        }
    }
}
