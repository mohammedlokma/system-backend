namespace system_backend.Models.Dtos
{
    public class AgentModel
    {
        public ICollection<CouponDTO> Coupons { get; set; }
        public ICollection<ExpensesPayments> Expenses { get; set; }

    }
}
