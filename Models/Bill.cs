namespace MobileBillingSystem.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public int SubscriberNo { get; set; }
        public double TotalAmount { get; set; }

        // Nullable yapılması önerilebilir.
        public string PaidStatus { get; set; } = string.Empty; // Default boş string verildi
        public int Month { get; set; }
        public int Year { get; set; }

        public Subscriber? Subscriber { get; set; }
    }
}
