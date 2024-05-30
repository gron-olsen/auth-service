using authServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;

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
            _config["Valid"] ?? "http://localhost",
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

        HttpResponseMessage response = await _httpClient.GetAsync("http://" + url + $"?username={login.UserName}&Password={login.Password}");

        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();


        User user = JsonConvert.DeserializeObject<User>(responseContent);

        if (login.UserName != user.UserName || login.Password != user.Password)
        {
            return Unauthorized();
        }

        _logger.LogInformation($"User: {login.UserName} has a been assigned a token!");

        var token = GenerateJwtToken(login.UserName);
        return Ok(token);
    }

    [HttpGet("Version")]
    public Dictionary<string, string> GetVersion()
    {
        var properties = new Dictionary<string, string>();
        var assembly = typeof(Program).Assembly;

        properties.Add("service", "Auth");
        var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).FileVersion ?? "Undefined";
        Console.WriteLine($"Version before: {ver}");
        properties.Add("version", ver);

        var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
        var localIPAddr = feature?.LocalIpAddress?.ToString() ?? "N/A";
        properties.Add("local-host-address", localIPAddr);

        return properties;
    }
}