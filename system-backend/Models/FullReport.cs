using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class FullReport
    {
        [Key]
        public int Id { get; set; }
        public string? CompanyComment { get; set; }
        public string? AgentComment { get; set; }
        public bool ReleaseStatus { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
