namespace MobileBillingSystem.DTO
{
    public class AddUsageDTO
    {
        public int SubscriberNo { get; set; }
        public string? UsageType { get; set; }
        public double Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

}
