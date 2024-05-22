using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);
    string myValidAudience = Environment.GetEnvironmentVariable("Valid")?? "http://localhost";
    string mySecret = Environment.GetEnvironmentVariable("Secret") ?? "none";
    string myIssuer = Environment.GetEnvironmentVariable("Issuer") ?? "none";

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = myIssuer,
            ValidAudience = myValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecret))
        };
    });
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (System.Exception ex)
{
    // Logging the exception and re-throwing it
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
