using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class AccountDto
    {
        public int AccountId { get; set; }

        [Required, MaxLength(20)]
        public string AccountNumber { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [MaxLength(100)]
        public string AccountType { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
