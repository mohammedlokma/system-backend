namespace system_backend.Models.Dtos
{
    public class CouponDTO
    {
        
        public int Id { get; set; }
        public string AgentId { get; set; }
       
        public float Price { get; set; }
        public string? Details { get; set; }
        public DateTime Date { get; set; }
        public int? BillNumber { get; set; }
        public int? CertificateNumber { get; set; }
        public int? CouponNumber { get; set; }
        public int? PolicyNumber { get; set; }
    }
}
