namespace system_backend.Models.Dtos
{
    public class AuthModel
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        
        public string UserDisplayName { get; set; }

        public List<string> Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
