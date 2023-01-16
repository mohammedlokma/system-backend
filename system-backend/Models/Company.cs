using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class Company
    {
        [Key]
        public string Id { get; set; }
        public ICollection<CompanyReportItems> ReportItems { get; set; }
        public double Account { get; set; }
        public ICollection<CompanyPayments> Payments { get; set; }
        public ICollection<Bills> Bills { get; set; }
    }
}
