namespace PhonebookService.Api.Core.Domain;

public class Person
{
    public Guid Id { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Firma { get; set; }
    public ICollection<ContactInfo> ContactInfos { get; set; } = new HashSet<ContactInfo>();
}