﻿namespace system_backend.Models
{
    public class CompanyReportItems
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CompanyId")]
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        [ForeignKey("ReportItemId")]
        public int ReportItemId { get; set; }
        public ReportItems ReportItems { get; set; }
    }
}
