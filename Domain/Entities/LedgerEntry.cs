using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace accountSystem.Domain.Entities
{
    public class LedgerEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LedgerEntryId { get; set; }

        [Required]
        [ForeignKey("Transaction")]
        public int TransactionId { get; set; }

        public int? JournalEntryId { get; set; } // Optional foreign key to JournalEntries

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // الخصائص المضافة لحساب المدين والدائن
        [Required]
        public decimal Amount { get; set; } // مبلغ المعاملة

        [Required]
        public bool IsDebit { get; set; } // تحديد إذا كانت المعاملة مدين (true) أو دائن (false)
        [Required]
        public decimal BalanceAfterTransaction { get; set; }// الرصيد بعد المعاملة

        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; } // خاصية AccountId لتحديد الحساب المرتبط بكل إدخال

        public Transaction Transaction { get; set; }
        public Account Account { get; set; } // الربط مع كيان الحساب
    }
}
