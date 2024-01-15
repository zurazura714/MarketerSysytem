namespace MarketerSystem.Domain.ResourceParameters
{
    public class PaymentFilterParameters
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
