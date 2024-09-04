using accountSystem.Application.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using accountSystem.Domain.Entities;

public class ReportController : Controller
{
    private readonly AccountBalanceService _accountBalanceService;

    public ReportController(AccountBalanceService accountBalanceService)
    {
        _accountBalanceService = accountBalanceService;
    }

    // View for printing options
    public async Task<IActionResult> ReportView()
    {
        var accountBalances = await _accountBalanceService.GetAccountBalancesAsync();

        // تحويل AccountBalanceDto إلى Account
        var accounts = accountBalances.Select(dto => new Account
        {
            AccountNumber = dto.AccountNumber,
            //AccountName = dto.AccountName,
            CurrentBalance = dto.CurrentBalance,
            AccountType = dto.AccountType,
            IsActive = dto.IsActive
        }).ToList();

        return View(accountBalances);
    }




    // Generate PDF report
    public async Task<IActionResult> GenerateAccountReportPdf()
    {
        var accountBalances = await _accountBalanceService.GetAccountBalancesAsync();

        using (MemoryStream stream = new MemoryStream())
        {
            // إعداد المستند
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
            writer.CloseStream = false;
            pdfDoc.Open();

            // إضافة عنوان للتقرير
            var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
            var title = new Paragraph("Account Report", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20f // مسافة بعد العنوان
            };
            pdfDoc.Add(title);

            if (accountBalances != null && accountBalances.Any())
            {
                // إنشاء جدول بـ 4 أعمدة
                PdfPTable table = new PdfPTable(4)
                {
                    WidthPercentage = 100, // عرض الجدول يكون 100% من عرض الصفحة
                    SpacingBefore = 10f // مسافة قبل الجدول
                };

                // إعداد عرض الأعمدة
                table.SetWidths(new float[] { 2f, 2f, 2f, 2f });

                // إضافة ترويسات الأعمدة
                AddTableHeader(table);

                // إضافة البيانات إلى الجدول
                foreach (var account in accountBalances)
                {
                    table.AddCell(account.AccountNumber);
                    table.AddCell(account.CurrentBalance.ToString("C"));
                    table.AddCell(account.AccountType);
                    table.AddCell(account.IsActive ? "Active" : "Inactive");
                }

                // إضافة الجدول إلى المستند
                pdfDoc.Add(table);
            }
            else
            {
                pdfDoc.Add(new Paragraph("No account balances available."));
            }

            pdfDoc.Close(); // إغلاق مستند PDF

            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", "AccountReport.pdf");
        }
    }

    // وظيفة لمساعدة على إضافة ترويسات الأعمدة إلى الجدول
    private void AddTableHeader(PdfPTable table)
    {
        var boldFont = FontFactory.GetFont("Arial", 12, Font.BOLD);

        PdfPCell cell = new PdfPCell(new Phrase("Account Number", boldFont))
        {
            BackgroundColor = BaseColor.LIGHT_GRAY,
            HorizontalAlignment = Element.ALIGN_CENTER
        };
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Balance", boldFont))
        {
            BackgroundColor = BaseColor.LIGHT_GRAY,
            HorizontalAlignment = Element.ALIGN_CENTER
        };
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Account Type", boldFont))
        {
            BackgroundColor = BaseColor.LIGHT_GRAY,
            HorizontalAlignment = Element.ALIGN_CENTER
        };
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Status", boldFont))
        {
            BackgroundColor = BaseColor.LIGHT_GRAY,
            HorizontalAlignment = Element.ALIGN_CENTER
        };
        table.AddCell(cell);
    }

    // Generate Excel report
    public async Task<IActionResult> GenerateAccountReportExcel()
    {
        var accountBalances = await _accountBalanceService.GetAccountBalancesAsync();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Account Report");

            worksheet.Cells[1, 1].Value = "Account Number";
            worksheet.Cells[1, 2].Value = "Balance";

            int row = 2;
            foreach (var account in accountBalances)
            {
                worksheet.Cells[row, 1].Value = account.AccountNumber;
                worksheet.Cells[row, 2].Value = account.CurrentBalance;
                row++;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AccountReport.xlsx");
        }
    }
}
