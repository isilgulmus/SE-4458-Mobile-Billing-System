namespace MobileBillingSystem.DTO
{
    public class QueryBillDetailedDTO
    {
        public int SubscriberNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        // Paging parameters
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
