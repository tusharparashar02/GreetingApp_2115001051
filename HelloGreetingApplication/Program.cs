using BusinessLayer.Interface;
using BusinessLayer.Service;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Configure NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Application is starting");

try
{
    // Add services to the container
    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddControllers();  // Adding controllers to the DI container

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
