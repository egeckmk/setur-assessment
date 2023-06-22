namespace ReportingService.Api.Core.Domain.Models;

public class Report
{
    public Guid Id { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.Now;
    public ReportStatus ReportStatus { get; set; } = ReportStatus.Hazirlaniyor;
    public DateTime? CompletedDate { get; set; }
    public string? FileUrl { get; set; } 
}

public enum ReportStatus
{
    Hazirlaniyor = 0,
    Tamamlandi = 1
}

