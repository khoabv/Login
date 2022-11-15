using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Login.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public DateTime? BirthDate { get; set; }
        [StringLength(255)]
        public string Address { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        [StringLength(128)]
        public string CreatedBy { get; set; }
        [StringLength(128)]
        public string UpdatedBy { get; set; }

    }
}
