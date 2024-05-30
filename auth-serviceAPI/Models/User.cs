namespace authServiceAPI.Models
{

    public class User
    {
        public int UserID { get; set; }
        public required string UserName { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}