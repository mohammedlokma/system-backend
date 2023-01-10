using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend.Repository.Interfaces
{
    public interface IAdminsRepository
    {
        Task<List<AdminDTO>> GetAdmins();
    }
}
