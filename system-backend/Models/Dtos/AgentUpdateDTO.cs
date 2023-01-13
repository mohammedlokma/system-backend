namespace system_backend.Models.Dtos
{
    public class AgentUpdateDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public List<ServicePlacesDTO> ServicePlaces { get; set; } = new List<ServicePlacesDTO>();
    }
}
