using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketerSystem.Domain.Model
{
    public class Sell
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey(nameof(DistributorID))]
        public int DistributorID { get; set; }
        public Distributor Distributor { get; set; }

        public DateTimeOffset SoldDate { get; set; } = DateTimeOffset.Now;

        [ForeignKey(nameof(ProductID))]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public decimal ProductPrice { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal ProductTotalPrice { get; set; }

    }
}
