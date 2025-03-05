using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;





var logger = LogManager.GetCurrentClassLogger();
logger.Info("Application is starting");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("SqlConnections");
    builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));
    // Add services to the container
    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();

    builder.Services.AddControllers();  // Adding controllers to the DI container

    // Configure NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add Swagger to the container
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Enable Swagger UI for API documentation
    app.UseSwagger();
    app.UseSwaggerUI();

    // Configure the HTTP request pipeline
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application failed to start");
    throw;
}
finally
{
    LogManager.Shutdown();  // Ensure logs are flushed on shutdown
}
