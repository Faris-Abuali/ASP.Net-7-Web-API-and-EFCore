using CollegeApp.MyLogging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Previous videos - Logging before SeriLog video
//builder.Logging.ClearProviders(); // clear all 4 providers: Console, Debug, EventSource, and EventLog
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

// --- SeriLog Logger Package
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Minute)
//    .CreateLogger();
// a log file will be generated every minute :)

//builder.Host.UseSerilog(); // <-- Only SeriLog will work. The built-in loggers will not.
builder.Logging.AddSerilog(); // <-- Use Serilog along with the built-in loggers.

// Add services to the container.

builder.Services
    .AddControllers()
    //.AddControllers(options => options.ReturnHttpNotAcceptable = true) // 406
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters(); // To accept XML response

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
/**
 * Whenever the IMyLogger is passed (injected) to a class's constructor, then we
 * want the passed object to be of type LogToFile class
 */
builder.Services.AddScoped<IMyLogger, LogToDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
