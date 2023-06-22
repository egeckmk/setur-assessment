namespace PhonebookService.Api.Core.Domain.Models;

public class ContactInfoDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public ContactType ContactType  { get; set; }
    public string ContactContent { get; set; }
}