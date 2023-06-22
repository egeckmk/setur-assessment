namespace PhonebookService.Api.Core.Domain.Models;

public class PersonDto
{
    public Guid Id { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Firma { get; set; }
    public List<ContactInfo> ContactInfos { get; set; }
}