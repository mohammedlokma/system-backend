using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class Safe
    {
        [Key]
        public int Id { get; set; }
        public double Total { get; set; }
    }
}
