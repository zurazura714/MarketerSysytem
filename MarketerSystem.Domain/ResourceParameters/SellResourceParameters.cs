namespace MarketerSystem.Domain.ResourceParameters;

public class SellResourceParameters
{
    public int? ProductID { get; set; }
    public int? DistributorID { get; set; }
    public DateTimeOffset? SoldDate { get; set; }
}
