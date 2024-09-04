using System.ComponentModel.DataAnnotations.Schema;

namespace accountSystem.Domain.Entities
{
    public class UserRoleAssignment
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("UserRole")]
        public int UserRoleId { get; set; }

        public User User { get; set; }
        public UserRole UserRole { get; set; }
    }
}
