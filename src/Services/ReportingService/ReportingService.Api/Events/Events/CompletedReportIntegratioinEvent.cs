using EventBus.Base.Events;
using ReportingService.Api.Core.Domain.Models;

namespace ReportingService.Api.Events.Events;

public class CompletedReportIntegrationEvent: IntegrationEvent
{
    public Guid ReportId { get; set; }
    public List<ReportData> Data { get; set; }

    public CompletedReportIntegrationEvent(Guid reportId, List<ReportData> data)
    {
        ReportId = reportId;
        Data = data;
    }
}