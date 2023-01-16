namespace system_backend.Models.Dtos
{
    public class BillsDTO
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public float Total { get; set; }
        public string? Details { get; set; }
        public DateTime Date { get; set; }
    }
}
