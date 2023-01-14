using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend.Repository.Interfaces
{
    public interface IAgentRepository : IBaseRepository<Agent>
    {
        Task DeleteAgentAsync(string id);
        Task<List<AgentDTO>> GetAgentsAsync();
        Task<AuthModel> RegisterUserAsync(RegisterAgentModel model);
        Task<AgentUpdateDTO> UpdateAsync(AgentUpdateDTO agentDTO);
    }
}
