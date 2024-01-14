using System.ComponentModel.DataAnnotations;

namespace MarketerSystem.Domain.Model
{
    public class Passport
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public DocumentType DocumentType { get; set; }
        [MaxLength(10)]
        public string DocumentSerie { get; set; }
        [MaxLength(10)]
        public string DocumentNumber { get; set; }
        [Required]
        public DateTimeOffset ReleaseDate { get; set; }
        [Required]
        public DateTimeOffset ExpirationDate { get; set; }
        [MaxLength(50)]
        public string PersonalNumber { get; set; }
        [MaxLength(100)]
        public string IssuingAgency { get; set;}

        public int DistributorID { get; set; }
        public Distributor Distributor { get; set; }

    }
}
