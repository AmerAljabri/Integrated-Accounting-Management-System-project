using accountSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace accountSystem.Controllers
{
    public class LedgerController : Controller
    {
        private readonly LedgerService _ledgerService;

        public LedgerController(LedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }
    
        // عرض دفتر الأستاذ مع فلاتر اختيارية
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string accountType)
        {
            var ledgerEntries = await _ledgerService.GetLedgerEntriesAsync(startDate, endDate, accountType);
            var ledgerSummary = await _ledgerService.GetLedgerSummaryAsync(startDate, endDate, accountType);

            ViewBag.Summary = ledgerSummary;
            return View(ledgerEntries);
        }

    }
}
