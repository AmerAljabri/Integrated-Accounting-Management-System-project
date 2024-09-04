using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace accountSystem.Domain.Entities
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        [Required]
        [MaxLength(100)]
        public string TransactionType { get; set; }
        [MaxLength(100)]
        public string ReferenceNumber { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }

        public ICollection<TransactionAccount> TransactionAccounts { get; set; }
        public ICollection<LedgerEntry> LedgerEntries { get; set; }
    }
}
