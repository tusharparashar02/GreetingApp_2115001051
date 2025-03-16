using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Middleware.GlobalExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JWT.Service;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Info("Application is starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("SqlConnections");
    builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();
    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<IUserRL, UserRL>();
    builder.Services.AddScoped<TokenService>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    // ? JWT Configuration
    var jwtKey = builder.Configuration["Jwt:Key"];
    var jwtIssuer = builder.Configuration["Jwt:Issuer"];
    var jwtAudience = builder.Configuration["Jwt:Audience"];

    if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
    {
        throw new ArgumentNullException("JWT configuration values are missing in appsettings.json");
    }

    // ? Add Authentication & Authorization
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.AddAuthorization();






    //Exception
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>(); // Register global exception filter
    });

    // Configure NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    // Configure the HTTP request pipeline.
    app.UseAuthentication(); // Ensure this is BEFORE UseAuthorization
    app.UseAuthorization();
    app.UseHttpsRedirection();
    //app.UseAuthorization();
    app.MapControllers();

    logger.Info("Application started successfully.");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application failed to start due to an exception.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
