using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO
{
    public class PassportDTO
    {
        public int ID { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentSerie { get; set; }
        public string DocumentNumber { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public string PersonalNumber { get; set; }
        public string IssuingAgency { get; set; }
        public int DistributorID { get; set; }
    }
}