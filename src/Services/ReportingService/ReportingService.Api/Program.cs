using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using ReportingService.Api.Events.EventHandlers;
using ReportingService.Api.Events.Events;
using ReportingService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<ReportRequestIntegrationEventHandler>();
builder.Services.AddTransient<CompletedReportIntegrationEventHandler>();

builder.Services.AddSingleton<IEventBus>(sp => EventBusFactory.Create( new EventBusConfig
{
    ConnectionRetryCount = 5,
    SubscriberClientAppName = "ReportingService",
    // DefaultTopicName = "PhoneBookTopicName",
    EventBusType = EventBusType.RabbitMQ,
    EventNameSuffix = "IntegrationEvent"
}, sp));


builder.Services.ConfigureDbContext(builder.Configuration);

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();

eventBus.Subscribe<ReportRequestIntegrationEvent, ReportRequestIntegrationEventHandler>();
eventBus.Subscribe<CompletedReportIntegrationEvent, CompletedReportIntegrationEventHandler>();

app.MigrateDbContext();

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