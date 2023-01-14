namespace system_backend.Models.Dtos
{
    public class ExpenseDTO
    {
        
        public int Id { get; set; }
        public string AgentId { get; set; }
       
        public float Price { get; set; }
        public string? Details { get; set; }
        public DateTime Date { get; set; }
       
    }
}
