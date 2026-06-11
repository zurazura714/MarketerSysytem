using System.ComponentModel.DataAnnotations;
using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO;

public class DistributorDTO
{
    public int DistributorID { get; set; }
    public Guid DistributorGuid { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public int PassportID { get; set; }
    public PassportDTO? Passport { get; set; }

    [MaxLength(50)]
    public string? GenerationLinker { get; set; }

    public int? RecomendatorID { get; set; }

    public ICollection<PictureDTO>? Pictures { get; set; }

    [Required]
    public ICollection<ContactInfoDTO> ContactInfos { get; set; } = [];

    [Required]
    public ICollection<AddressDTO> Addresses { get; set; } = [];
}
