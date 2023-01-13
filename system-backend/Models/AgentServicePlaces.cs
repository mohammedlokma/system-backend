using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace system_backend.Models
{
    public class AgentServicePlaces
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AgentId")]
        public string AgentId { get; set; }
        public Agent Agent { get; set; }
        [ForeignKey("ServicePlacesId")]
        public int ServicePlacesId { get; set; }
        public ServicePlaces ServicePlaces { get; set; }

    }
}
