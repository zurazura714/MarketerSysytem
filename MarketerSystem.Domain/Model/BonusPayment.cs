using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketerSystem.Domain.Model
{
    public class BonusPayment
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public decimal BonusPay { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }

        [ForeignKey(nameof(Distributor))]
        public int DistributorID { get; set; }

        public virtual Distributor Distributor { get; set; }

    }
}
