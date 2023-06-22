using EventBus.Base.Events;
using PhonebookService.Api.Core.Domain.Models;

namespace PhonebookService.Api.Events.Events;

public class CompletedReportIntegrationEvent: IntegrationEvent
{
    public Guid ReportId { get; private set; }
    public List<ReportData> Data { get; private set; }

    public CompletedReportIntegrationEvent()
    {
        ReportId = Guid.NewGuid();
        Data = new List<ReportData>();
    }
    
    public CompletedReportIntegrationEvent(Guid reportId, List<ReportData> data)
    {
        ReportId = reportId;
        Data = data;
    }
}