using MarketerSystem.Common.Enums;

namespace MarketerSystem.Common.DTO;

public class PassportCreateDTO
{
    public DocumentType DocumentType { get; set; }
    public string DocumentSerie { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTimeOffset ReleaseDate { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public string PersonalNumber { get; set; } = string.Empty;
    public string IssuingAgency { get; set; } = string.Empty;
}
