namespace MobileBillingSystem.Models
{
    public class Subscriber
    {
        public int SubscriberNo { get; set; }  

        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Bill> Bills { get; set; }  
        public ICollection<Usage> Usages { get; set; }  
    }
}
