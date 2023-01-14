using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend.Repository.Interfaces
{
    public interface IAdminsRepository : IBaseRepository<ApplicationUser>
    {
        Task<List<AdminDTO>> GetAdmins();
    }
}
