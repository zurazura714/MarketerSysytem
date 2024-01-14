using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketerSystem.Domain.Model
{
    public class Picture
    {
        [Key]
        public int ID { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public DateTimeOffset UploadTime { get; set; }

        [ForeignKey(nameof(Distributor))]
        public int DistributorID { get; set;}
        public Distributor Distributor { get; set;}
    }
}
