using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketerSystem.Domain.Model
{
    public class Address
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public AddressType AddressType { get; set; }
        [Required, MaxLength(100)]
        public string AddressInfo { get; set; }
        [Required]
        public int DistributorID { get; set; }
        [ForeignKey(nameof(DistributorID))]
        public Distributor Distributor { get; set; }
    }
}
