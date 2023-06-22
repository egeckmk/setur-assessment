using EventBus.Base.Abstraction;
using PhonebookService.Api.Core.Domain;
using PhonebookService.Api.Core.Domain.Models;
using PhonebookService.Api.Events.Events;
using PhonebookService.Api.Infrastructure.Context;

namespace PhonebookService.Api.Events.EventHandlers;

public class PrepareReportIntegrationEventHandler : IIntegrationEventHandler<PrepareReportIntegrationEvent>
{
    private readonly PhoneBookContext _context;
    private readonly IEventBus _eventBus;

    public PrepareReportIntegrationEventHandler(PhoneBookContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }
    
    public Task Handle(PrepareReportIntegrationEvent @event)
    {
        var query = _context.Persons
            .Join(_context.ContactInfos,
                person => person.Id,
                cinfo => cinfo.PersonId,
                (person, cinfo) => new { person, cinfo })
            .Where(x => x.cinfo.ContactType == ContactType.Location)
            .GroupJoin(_context.ContactInfos,
                x => x.person.Id,
                cinfo2 => cinfo2.PersonId,
                (x, cinfo2) => new { x, cinfo2 })
            .SelectMany(x => x.cinfo2.DefaultIfEmpty(),
                (x, cinfo2) => new { x.x.person, x.x.cinfo, cinfo2 })
            .Where(x => x.cinfo2 == null || x.cinfo2.ContactType == 0)
            .GroupBy(x => x.cinfo.ContactContent)
            .Select(x => new ReportData
            {
                Location = x.Key,
                PersonCount = x.Count(y => y.person != null),
                PhoneCount = x.Count(y => y.cinfo2 != null)
            });

        var result = query.ToList();
        
        _eventBus.Publish(new CompletedReportIntegrationEvent(@event.ReportId, result));
        
        return Task.CompletedTask;
    }
}