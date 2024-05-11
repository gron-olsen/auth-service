using Microsoft.AspNetCore.Mvc;
using authServiceAPI.Models;

namespace authServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthServiceController : ControllerBase
{
    private readonly ILogger<AuthServiceController> _logger;

    public AuthServiceController(ILogger<AuthServiceController> logger)
    {
        _logger = logger;
    }

    [HttpPost("validateUser")]
    
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

