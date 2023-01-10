using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class ServicePlaces
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Agent> Agents { get; set; }

    }
}
