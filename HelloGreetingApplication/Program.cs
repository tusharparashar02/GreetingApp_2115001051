using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Middleware.GlobalExceptionHandler;

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

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    //Exception
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionHandler>(); // Register global exception filter
    });
    // Add other services
    builder.Services.AddScoped<GlobalExceptionHandler>();

    // Configure NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseAuthorization();
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
