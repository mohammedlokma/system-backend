namespace system_backend.Models.Dtos
{
    public class CompanyReportItemsDTO
    {
        public string CompanyId { get; set; }
        public List<CompanyReportItemsCreateDTO> CompanyReportItems { get; set; }
            = new List<CompanyReportItemsCreateDTO>();

    }
}
