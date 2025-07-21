using System.Text.Json.Serialization;
using BmsIngest.Scheduler;
using BmsIngest.Services.BmsRetrieval;
using BmsIngest.Services.InfluxDb;
using BmsIngest.Services.InsertionManager;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.Configure<BmsRetrievalServiceOptions>(builder.Configuration.GetSection("Bms"));
builder.Services.Configure<InfluxDbServiceOptions>(builder.Configuration.GetSection("InfluxDb"));

// Add HTTP client for BMS system with standard resilience handlers
builder.Services.AddHttpClient<IBmsRetrievalService, BmsRetrievalService>().AddStandardResilienceHandler();

// Add services
builder.Services.AddSingleton<IInfluxDbService, InfluxDbService>();
builder.Services.AddScoped<IBmsRetrievalService, BmsRetrievalService>();
builder.Services.AddScoped<IInsertionManagerService, InsertionManagerService>();

builder.Services.AddSchedulerService();

// Add Controllers
// Set Json options for string enums, and to ignore cyclic references
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Add swagger if in Dev environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();