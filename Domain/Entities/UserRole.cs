
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace accountSystem.Domain.Entities
{
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public string Permissions { get; set; }

        public ICollection<UserRoleAssignment> Users { get; set; }
    }
}
