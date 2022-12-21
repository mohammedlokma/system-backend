using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string UserDisplayName { get; set; }

    }
}
