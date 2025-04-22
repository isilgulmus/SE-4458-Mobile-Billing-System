namespace MobileBillingSystem.Models
{
    public class Usage
    {
        public int UsageId { get; set; }
        public int SubscriberNo { get; set; }
        public string UsageType { get; set; } = string.Empty; 
        public double Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }


        public Subscriber? Subscriber { get; set; }
    }
}
