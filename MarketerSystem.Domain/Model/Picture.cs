namespace MarketerSystem.Domain.Model
{
    public class Picture
    {
        public int PictureId { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public DateTimeOffset UploadTime { get; set; }
    }
}
