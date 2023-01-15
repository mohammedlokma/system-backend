using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class ReportItems
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ArabicName { get; set; }
        [Required]
        public string Type { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}
