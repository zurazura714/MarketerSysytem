namespace MarketerSystem.Common.DTO
{
    public class PictureCreateDTO
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public DateTimeOffset UploadTime { get; set; }
    }
}