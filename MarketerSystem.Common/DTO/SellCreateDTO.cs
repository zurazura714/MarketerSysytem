namespace MarketerSystem.Common.DTO
{
    public class SellCreateDTO
    {
        public int DistributorID { get; set; }
        public DateTimeOffset SoldDate { get; set; }
        public int ProductID { get; set; }
    }
}