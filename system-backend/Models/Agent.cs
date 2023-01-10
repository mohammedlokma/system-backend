using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace system_backend.Models
{
    public class Agent : IdentityUser
    {
        [Required, MaxLength(50)]
        public string UserDisplayName { get; set; }
        public float custody { get; set; }
        public ICollection<ServicePlaces> ServicePlaces { get; set; }
        public ICollection<ExpensesPayments> Expenses { get; set; }
        public ICollection<CouponsPayments> Coupons { get; set; }

    }
}
