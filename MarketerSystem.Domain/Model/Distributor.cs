using MarketerSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Domain.Model
{
    public class Distributor
    {
        [Key]
        public int DistributorID { get; set; }
        public Guid DistributorGuid { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }


        [ForeignKey(nameof(Passport))]
        public int PassportID { get; set; }
        public Passport Passport { get; set; }


        [MaxLength(50)]
        public string? GenerationLinker { get; set; }

        [ForeignKey(nameof(DistributorID))]
        public int? RecomendatorID { get; set; }
        public Distributor Recomendator { get; set; }


        public virtual ICollection<Picture>? Pictures { get; set; }
        [Required]
        public virtual ICollection<ContactInfo> ContactInfos { get; set; }
        [Required]
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<BonusPayment> BonusPayments { get; set; }

    }
}
