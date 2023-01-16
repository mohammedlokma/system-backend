namespace system_backend.Models.Dtos
{
    public class AgentModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public float custody { get; set; }
        public List<string> ServicePlaces { get; set; }
        public ICollection<CouponDTO> Coupons { get; set; }
        public ICollection<ExpenseDTO> Expenses { get; set; }

    }
}
