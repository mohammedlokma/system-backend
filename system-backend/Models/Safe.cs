using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace system_backend.Models
{
    public class Safe
    {
        [Key]
        public int Id { get; set; }
        public double Total { get; set; }
    }
}
