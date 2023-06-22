using ClosedXML.Excel;
using EventBus.Base.Abstraction;
using ReportingService.Api.Core.Domain.Models;
using ReportingService.Api.Events.Events;
using ReportingService.Api.Infrastructure.Context;

namespace ReportingService.Api.Events.EventHandlers;

public class CompletedReportIntegrationEventHandler : IIntegrationEventHandler<CompletedReportIntegrationEvent>
{
    private readonly IWebHostEnvironment _environment;
    private readonly ReportingContext _context;

    public CompletedReportIntegrationEventHandler(IWebHostEnvironment environment, ReportingContext context)
    {
        _environment = environment;
        _context = context;
    }
    public Task Handle(CompletedReportIntegrationEvent @event)
    {
        
        var entity = _context.Reports.Find(@event.ReportId);
        if (entity == null)
        {
            return Task.CompletedTask;
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Report");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = nameof(ReportData.Location);
            worksheet.Cell(currentRow, 2).Value = nameof(ReportData.PersonCount);
            worksheet.Cell(currentRow, 3).Value = nameof(ReportData.PhoneCount);

            foreach (var data in @event.Data)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = data.Location;
                worksheet.Cell(currentRow, 2).Value = data.PersonCount;
                worksheet.Cell(currentRow, 3).Value = data.PhoneCount;
            }

            var filesPath = Path.Combine(_environment.ContentRootPath, "Files");
            if (!Directory.Exists(filesPath))
                Directory.CreateDirectory(filesPath);
            var filePath = Path.Combine(filesPath, @event.ReportId + ".xlsx");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            workbook.SaveAs(fileStream);

            entity.FileUrl = filePath;
            entity.ReportStatus = ReportStatus.Tamamlandi;
            entity.CompletedDate = DateTime.Now;
            _context.SaveChanges();
        }
        
        return Task.CompletedTask;
    }
}