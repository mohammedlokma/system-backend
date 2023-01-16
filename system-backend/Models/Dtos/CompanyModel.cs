namespace system_backend.Models.Dtos
{
    public class CompanyModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public double Account { get; set; }
        public ICollection<PaymentsDTO> Payments { get; set; }
        public ICollection<BillsDTO> Bills { get; set; }
    }
}
