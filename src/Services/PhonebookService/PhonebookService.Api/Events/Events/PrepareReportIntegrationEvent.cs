using EventBus.Base.Events;

namespace PhonebookService.Api.Events.Events;

public class PrepareReportIntegrationEvent : IntegrationEvent
{
    public Guid ReportId { get; set; }

    public PrepareReportIntegrationEvent(Guid reportId)
    {
        ReportId = reportId;
    }
}