using System.ComponentModel.DataAnnotations;

namespace system_backend.Models.Dtos
{
    public class PasswordResetDto
    {
        
        [Required]
        public string userId { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string ConfirmPassword { get; set; }
    }
}
