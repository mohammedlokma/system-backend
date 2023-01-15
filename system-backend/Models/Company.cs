using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public ICollection<ReportItems> reportItems { get; set; }
        public double Account { get; set; }
    }
}
