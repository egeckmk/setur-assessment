using EventBus.Base.Abstraction;
using ReportingService.Api.Core.Domain.Models;
using ReportingService.Api.Events.Events;
using ReportingService.Api.Infrastructure.Context;

namespace ReportingService.Api.Events.EventHandlers;

public class ReportRequestIntegrationEventHandler : IIntegrationEventHandler<ReportRequestIntegrationEvent>
{
    private readonly ReportingContext _context;
    private readonly IEventBus _eventBus;

    public ReportRequestIntegrationEventHandler(ReportingContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }
    
    public Task Handle(ReportRequestIntegrationEvent @event)
    {
        var report = new Report();

        _context.Reports.Add(report);
        _context.SaveChanges();
        
        _eventBus.Publish(new PrepareReportIntegrationEvent(report.Id));
        return Task.CompletedTask;
    }
}