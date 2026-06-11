using System.ComponentModel.DataAnnotations;
using MarketerSystem.Common.Enums;

namespace MarketerSystem.Domain.Model;

public class Passport
{
    [Key]
    public int ID { get; set; }

    [Required]
    public DocumentType DocumentType { get; set; }

    [MaxLength(10)]
    public string DocumentSerie { get; set; } = string.Empty;

    [MaxLength(10)]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required]
    public DateTimeOffset ReleaseDate { get; set; }

    [Required]
    public DateTimeOffset ExpirationDate { get; set; }

    [MaxLength(50)]
    public string PersonalNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string IssuingAgency { get; set; } = string.Empty;

    public int DistributorID { get; set; }
    public virtual Distributor? Distributor { get; set; }
}
