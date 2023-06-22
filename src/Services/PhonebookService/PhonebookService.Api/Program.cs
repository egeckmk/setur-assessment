using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PhonebookService.Api.Events.EventHandlers;
using PhonebookService.Api.Events.Events;
using PhonebookService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<PrepareReportIntegrationEventHandler>();

builder.Services.AddSingleton<IEventBus>(sp => EventBusFactory.Create( new EventBusConfig
{
    ConnectionRetryCount = 5,
    SubscriberClientAppName = "PhonebookService",
    // DefaultTopicName = "PhoneBookTopicName",
    EventBusType = EventBusType.RabbitMQ,
    EventNameSuffix = "IntegrationEvent"
}, sp));


builder.Services.ConfigureDbContext(builder.Configuration);

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();

eventBus.Subscribe<PrepareReportIntegrationEvent, PrepareReportIntegrationEventHandler>();

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