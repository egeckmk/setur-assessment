using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingService.Api.Core.Domain.Models;
using ReportingService.Api.Events.Events;
using ReportingService.Api.Infrastructure.Context;

namespace ReportingService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportingController : ControllerBase
{
    
    private readonly ReportingContext _context;
    private readonly IEventBus _eventBus;

    public ReportingController(ReportingContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    [HttpGet]
    [Route("systemCheck")]
    public async Task<IActionResult> SystemCheck()
    {
        return Ok("System Online");
    }
    
    
    [HttpGet]
    [Route("reporting")]
    public async Task<IActionResult> Reports()
    {
        var reports = await _context.Reports.ToListAsync();
        if (!reports.Any())
        {
            return NotFound("No saved reports found.");
        }
        return Ok(reports);
    }
    
    [HttpGet]
    [Route("reporting/reportId")]
    public async Task<IActionResult> ReportDetail(Guid reportId)
    {
        var report = await _context.Reports.FindAsync(reportId);
        if (report == null)
        {
            return NotFound("Report not found.");
        }
        
        return Ok(report);
    }
}
