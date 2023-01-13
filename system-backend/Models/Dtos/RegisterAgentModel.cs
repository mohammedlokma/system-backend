using System.ComponentModel.DataAnnotations;

namespace system_backend.Models.Dtos
{
    public class RegisterAgentModel
    {
        public string? Id { get; set; }
        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, StringLength(100)]
        public string UserDisplayName { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; } = "User";
        public float custody { get; set; }
        public List<ServicePlacesDTO> ServicePlaces { get; set; } = new List<ServicePlacesDTO>();
    }
}
