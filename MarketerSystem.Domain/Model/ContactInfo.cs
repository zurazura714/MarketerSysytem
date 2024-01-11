using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketerSystem.Domain.Model
{
    public class ContactInfo
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public ContactInformationType ContactInformationType { get; set; }

        [Required, MaxLength(100)]
        public string Information { get; set; }

        [Required]
        public int DistributorID { get; set; }
        [ForeignKey(nameof(DistributorID))]
        public Distributor Distributor { get; set; }
    }

}
