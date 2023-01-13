using System.ComponentModel.DataAnnotations;

namespace system_backend.Models.Dtos
{
    public class AgentDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public List<string> ServicePlaces { get; set; } = new List<string>();

    }
}
