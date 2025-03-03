using NLog;
using NLog.Web;
var builder = WebApplication.CreateBuilder(args);


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Application is starting");

// Add services to the container.

builder.Services.AddControllers();

//Add swagger to container

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); //configer the Http request pipeline

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
