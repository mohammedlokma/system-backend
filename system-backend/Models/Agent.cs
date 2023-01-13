using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class Agent 
    {
        [Key]
        public string Id { get; set; }
        public float custody { get; set; }
        public ICollection<AgentServicePlaces> ServicePlaces { get; set; }
        public ICollection<ExpensesPayments> Expenses { get; set; }
        public ICollection<CouponsPayments> Coupons { get; set; }

    }
}
