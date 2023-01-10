using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace system_backend.Models
{
    public class ExpensesPayments
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Agent")]
        public string AgentId { get; set; }
        public Agent Agent { get; set; }

        [Required]
        public float Price { get; set; }
        public string? Details { get; set; }
        [Required]
        public DateTime Date { get; set; }

    }
}
