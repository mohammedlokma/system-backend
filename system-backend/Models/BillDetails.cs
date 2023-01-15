using System.ComponentModel.DataAnnotations.Schema;

namespace system_backend.Models
{
    public class BillDetails
    {
        [Key]
        public int MyProperty { get; set; }
        [ForeignKey("BillId")]
        public int BillId { get; set; }
        public Bills Bill { get; set; }
        [Required]
        public float Price { get; set; }
        public string? Details { get; set; }

    }
}
