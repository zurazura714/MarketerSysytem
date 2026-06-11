using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketerSystem.Common.Enums;

namespace MarketerSystem.Domain.Model;

public class Distributor
{
    [Key]
    public int DistributorID { get; set; }
    public Guid DistributorGuid { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [ForeignKey(nameof(Passport))]
    public int PassportID { get; set; }
    public Passport? Passport { get; set; }

    [MaxLength(50)]
    public string? GenerationLinker { get; set; }

    [ForeignKey(nameof(DistributorID))]
    public int? RecomendatorID { get; set; }
    public virtual Distributor? Recomendator { get; set; }

    public virtual ICollection<Picture>? Pictures { get; set; }

    [Required]
    public virtual ICollection<ContactInfo> ContactInfos { get; set; } = [];

    [Required]
    public virtual ICollection<Address> Addresses { get; set; } = [];

    public virtual ICollection<BonusPayment>? BonusPayments { get; set; }
}
