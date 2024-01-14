using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO
{
    public class ContactInfoCreateDTO
    {
        public ContactInformationType ContactInformationType { get; set; }
        public string Information { get; set; }
    }
}