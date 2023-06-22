using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhonebookService.Api.Core.Domain;
using PhonebookService.Api.Core.Domain.Models;
using PhonebookService.Api.Events.Events;
using PhonebookService.Api.Infrastructure.Context;

namespace PhonebookService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhonebookController : ControllerBase
{

    private readonly PhoneBookContext _context;
    private readonly IEventBus _eventBus;

    public PhonebookController(PhoneBookContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }
    
    [HttpGet]
    [Route("systemCheck")]
    public async Task<IActionResult> SystemCheck()
    {
        // SELECT cinfo.ContactContent AS [LOCATION], count(person.Id) as PERSON_COUNT, count(cinfo2.Id) as PHONE_COUNT 
        // FROM PhoneBookApp.person.Person person
        // inner join PhoneBookApp.person.ContactInfo cinfo on person.Id = cinfo.PersonId and cinfo.ContactType = 2
        // LEFT join PhoneBookApp.person.ContactInfo cinfo2 on person.Id = cinfo2.PersonId and cinfo2.ContactType = 0 
        // GROUP BY cinfo.ContactContent
        
        
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
        
        return Ok(result);
    }

    [HttpPost]
    [Route("persons")]
    public async Task<IActionResult> AddPerson([FromBody] Person person)
    {

        var personExist = await _context.Persons.AnyAsync(p => p.Ad == person.Ad && p.Soyad == person.Soyad);

        if (personExist)
        {
            return Conflict("Person already exists");
        }
         
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
        
        return Ok(person);
    }
    
    [HttpDelete]
    [Route("persons")]
    public async Task<IActionResult> DeletePerson([FromBody] Guid guid)
    {

        var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == guid);

        if (person == null)
        {
            return NotFound("Person not found.");
        }

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        
        return Ok("Person deleted.");
    }
    
    [HttpGet]
    [Route("persons")]
    public async Task<IActionResult> GetPersons()
    {

        var persons = await _context.Persons.ToListAsync();

        if (persons == null)
        {
            return NotFound("There is no user in your contacts yet..");
        }
        
        return Ok(persons);
    }
    
    [HttpPost]
    [Route("contactInfos")]
    public async Task<IActionResult> AddContactInfo([FromBody] ContactInfoDto contactInfo)
    {
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == contactInfo.PersonId);

        if (person == null)
        {
            return NotFound("Person not found.");
        }

        var entity = new ContactInfo()
        {
            ContactType = contactInfo.ContactType,
            ContactContent = contactInfo.ContactContent
        };
        person.ContactInfos.Add(entity);
        
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet]
    [Route("persons/{guid}")]
    public async Task<IActionResult> GetPersonDetail(Guid guid)
    {
        var persons = await _context.Persons.FindAsync(guid);
 
        if (persons == null)
        {
            return NotFound("Person not found.");
        }
        
        return Ok(persons);
    }
    
    [HttpGet]
    [Route("reportRequest")]
    public async Task<IActionResult> ReportRequest()
    {
        _eventBus.Publish(new ReportRequestIntegrationEvent());
        return Ok();
    }
}