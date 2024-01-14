using System.ComponentModel.DataAnnotations;
using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO
{
    public class DistributorCreateDTO
    {

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

        public PassportCreateDTO Passport { get; set; }

        public int? RecomendatorID { get; set; }

        public ICollection<PictureCreateDTO> Pictures { get; set; }

        [Required]
        public ICollection<ContactInfoCreateDTO> ContactInfos { get; set; }

        [Required]
        public ICollection<AddressCreateDTO> Addresses { get; set; }
    }
}