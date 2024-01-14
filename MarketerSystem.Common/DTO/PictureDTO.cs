namespace MarketerSystem.Common.DTO
{
    public class PictureDTO
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public DateTimeOffset UploadTime { get; set; }
        public int DistributorID { get; set; }
    }
}