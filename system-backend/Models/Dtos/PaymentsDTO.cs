namespace system_backend.Models.Dtos
{
    public class PaymentsDTO
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public float Price { get; set; }
        public string? Details { get; set; }
        public DateTime Date { get; set; }
    }
}
