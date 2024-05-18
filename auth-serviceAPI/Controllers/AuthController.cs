using authServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace authServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly IConfiguration _config;

    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger, IConfiguration config)
    {
        _config = config;
        _logger = logger;
    }
    private string GenerateJwtToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Secret"] ?? string.Empty));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);        
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, username)
            };

        var token = new JwtSecurityToken(
            _config["Issuer"],
            _config["ValidAudience"] ?? "http://localhost",
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        string url = _config["apiGetUser"] ?? string.Empty;
        _logger.LogInformation("Attempting to login to: " + "http://" + url + $" as User:{login.UserName}");
       
        HttpResponseMessage response = await _httpClient.GetAsync("http://" + url + $"?username={login.UserName}&Password={login.Password}" );
        
        string responseContent = await response.Content.ReadAsStringAsync();
       
        _logger.LogInformation($"Response: {responseContent}");
       
        User user = JsonConvert.DeserializeObject<User>(responseContent);

        if (login.UserName != user.UserName || login.Password != user.Password)
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(login.UserName);
       
        _logger.LogInformation($"User: {login.UserName} has a been assigned a token!");
       
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateJwtToken([FromBody] string? token)
    {
        if (token.IsNullOrEmpty())
            return BadRequest("Invalid token submitted.");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Secret"]!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var accountId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return Ok(accountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(404);
        }
    }

    [HttpGet("version")]
    public IEnumerable<string> Get()
    {
        var properties = new List<string>();
        var assembly = typeof(Program).Assembly;
        foreach (var attribute in assembly.GetCustomAttributesData())
        {
            properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()}");
        }
        return properties;
    }


}

