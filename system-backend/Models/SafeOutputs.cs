namespace system_backend.Models
{
    public class SafeOutputs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string details { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
