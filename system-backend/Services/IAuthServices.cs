using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend.Services
{
    public interface IAuthServices
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<ApplicationUser> ResetPasswordAsync(PasswordResetDto passwordResetDto);
    }
}
