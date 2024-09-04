using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace accountSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // خصائص إضافية مخصصة
        public DateTime? LastLogin { get; set; } // Optional

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // العلاقة مع جدول UserRoleAssignment
        public ICollection<UserRoleAssignment> UserRoles { get; set; }
    }
}
