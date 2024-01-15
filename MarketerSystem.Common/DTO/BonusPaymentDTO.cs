namespace MarketerSystem.Common.DTO
{
    public class BonusPaymentDTO
    {
        public int ID { get; set; }
        public decimal BonusPay { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public int DistributorID { get; set; }
        public DistributorDTO Distributor { get; set; }
    }
}