namespace authServiceAPI.Models
{

public class User
{
    public int UserID { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string Address { get; set; }
}
}