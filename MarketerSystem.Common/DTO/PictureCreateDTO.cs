namespace MarketerSystem.Common.DTO;

public class PictureCreateDTO
{
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = [];
    public DateTimeOffset UploadTime { get; set; }
}
