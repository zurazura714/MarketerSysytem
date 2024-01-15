namespace MarketerSystem.Common.DTO
{
    public class SellDTO
    {
        public int ID { get; set; }
        public int DistributorID { get; set; }
        public DateTimeOffset SoldDate { get; set; }
        public int ProductID { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal ProductTotalPrice { get; set; }

        public DistributorDTO Distributor { get; set; }
        public ProductDTO Product { get; set; }
    }
}