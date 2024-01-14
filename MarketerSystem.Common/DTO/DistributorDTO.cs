using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO
{
    public class DistributorDTO
    {
        public int DistributorID { get; set; }
        public Guid DistributorGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string GenerationLinker { get; set; }
        public int? RecomendatorID { get; set; }

        public PictureDTO Picture { get; set; }
        public PassportDTO Passport { get; set; }

        public ICollection<ContactInfoDTO> ContactInfos { get; set; }
        public ICollection<AddressDTO> Addresses { get; set; }
        public ICollection<BonusPaymentDTO> BonusPayments { get; set; }
    }

    public class PictureDTO
    {
        // Define properties for ContactInfo DTO
    }
    public class PassportDTO
    {
        // Define properties for ContactInfo DTO
    }
    public class ContactInfoDTO
    {
        // Define properties for ContactInfo DTO
    }

    public class AddressDTO
    {
        // Define properties for Address DTO
    }

    public class BonusPaymentDTO
    {
        // Define properties for BonusPayment DTO
    }
}