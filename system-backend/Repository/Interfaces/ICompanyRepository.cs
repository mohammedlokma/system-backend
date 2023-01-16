using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend.Repository.Interfaces
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task DeleteCompanyAsync(string id);
        Task<List<CompanyDTO>> GetCompaniesAsync();
        Task<CompanyModel> GetCompanyAsync(string id);
        Task<AuthModel> RegisterUserAsync(RegisterCompanyModel model);
        Task<CompanyUpdateModel> UpdateAsync(CompanyUpdateModel companyDTO);
    }
}
