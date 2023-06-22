namespace PhonebookService.Api.Core.Domain;

public class ContactInfo
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public ContactType ContactType  { get; set; }
    public string ContactContent { get; set; }
}

public enum ContactType {
    PhoneNumber = 0,
    EmailAddress = 1,
    Location = 2,
}