﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace system_backend.Models
{
    public class Bills
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CompanyId")]
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        [Required]
        public float Total { get; set; }
        public string? Details { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public ICollection<BillDetails> BillDetails { get; set; }
    }
}
