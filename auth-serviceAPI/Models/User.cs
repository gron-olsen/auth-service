namespace authServiceAPI.Models
{

public class User
{
    public int UserID { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public string Token { get; set; } = string.Empty;
}
}