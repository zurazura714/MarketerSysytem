using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO
{
    public class AddressCreateDTO
    {
        public AddressType AddressType { get; set; }
        public string AddressInfo { get; set; }
    }
}