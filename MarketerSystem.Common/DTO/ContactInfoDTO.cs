using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO
{
    public class ContactInfoDTO
    {
        public int ID { get; set; }
        public ContactInformationType ContactInformationType { get; set; }
        public string Information { get; set; }
        public int DistributorID { get; set; }
    }
}