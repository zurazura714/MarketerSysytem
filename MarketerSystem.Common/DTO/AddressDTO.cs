using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO
{
    public class AddressDTO
    {
        public int ID { get; set; }
        public AddressType AddressType { get; set; }
        public string AddressInfo { get; set; }
        public int DistributorID { get; set; }
    }
}