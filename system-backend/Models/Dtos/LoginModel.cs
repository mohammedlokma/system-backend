using Microsoft.Build.Framework;

namespace system_backend.Models.Dtos
{
    public class LoginModel
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
