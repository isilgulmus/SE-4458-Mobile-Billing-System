using Microsoft.EntityFrameworkCore;
using MobileBillingSystem.Data;
using MobileBillingSystem.DTO;
using MobileBillingSystem.DTO.MobileBillingSystem.DTOs;
using MobileBillingSystem.Models;
using MobileBillingSystem.Service;

namespace MobileBillingSystem.Service
{
    public class BillService
    {
        private readonly YourDbContext _context;

        public BillService(YourDbContext context)
        {
            _context = context;
        }

        public async Task<string> CalculateBill(CalculateBillDTO dto)
        {
            var subscriber = await _context.Subscribers
                .Where(s => s.SubscriberNo == dto.SubscriberNo)
                .FirstOrDefaultAsync();

            if (subscriber == null) return "Subscriber not found";

            var usages = await _context.Usages
                .Where(u => u.SubscriberNo == dto.SubscriberNo && u.Month == dto.Month && u.Year == dto.Year)
                .ToListAsync();

            double totalAmount = 0;

            // Telefon hesaplaması
            var phoneMinutes = usages.Where(u => u.UsageType == "phone").Sum(u => u.Amount);
            if (phoneMinutes > 1000)
            {
                totalAmount += (phoneMinutes - 1000) / 1000 * 10;
            }

            // İnternet hesaplaması
            var internetAmount = usages.Where(u => u.UsageType == "internet").Sum(u => u.Amount);
            if (internetAmount > 20)
            {
                totalAmount += (internetAmount - 20) / 10 * 10;
            }

            // Bill kaydetme
            var bill = new Bill
            {
                SubscriberNo = dto.SubscriberNo,
                Month = dto.Month,
                Year = dto.Year,
                TotalAmount = totalAmount,
                PaidStatus = "Unpaid"
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return $"Total Bill: {totalAmount} USD";

        }
        public async Task<string> QueryBill(QueryBillDTO dto)
        {
            var bill = await _context.Bills
                   .Where(b => b.SubscriberNo == dto.SubscriberNo && b.Month == dto.Month && b.Year == dto.Year)
                   .FirstOrDefaultAsync();

            if (bill == null) return "Bill not found";

            return $"Total Bill: {bill.TotalAmount} USD, Paid Status: {bill.PaidStatus}";
        }
        private double CalculatePhoneCharge(double phoneMinutes)
        {
            if (phoneMinutes <= 1000) return 0;  // İlk 1000 dakika ücretsiz
            return Math.Ceiling((phoneMinutes - 1000) / 1000.0) * 10;  // Her 1000 dakika için 10 USD
        }

        private double CalculateInternetCharge(double internetAmount)
        {
            if (internetAmount <= 20) return 0;  // İlk 20 GB ücretsiz
            return Math.Ceiling((internetAmount - 20) / 10.0) * 10;  // Her 10 GB için 10 USD
        }
        public async Task<string> QueryBillDetailed(QueryBillDetailedDTO dto)
        {
            var bill = await _context.Bills
     .Where(b => b.SubscriberNo == dto.SubscriberNo && b.Month == dto.Month && b.Year == dto.Year)
     .FirstOrDefaultAsync();

            if (bill == null)
            {
                return "Bill not found"; // Fatura bulunmazsa
            }
            // Filter usages for the specific subscriber, month, and year
            var usagesQuery = _context.Usages
                .Where(u => u.SubscriberNo == dto.SubscriberNo && u.Month == dto.Month && u.Year == dto.Year);

            // Apply pagination (Skip and Take)
            var totalUsages = await usagesQuery.CountAsync();  // Get the total number of records
            var usages = await usagesQuery
                .Skip((dto.PageNumber - 1) * dto.PageSize)  // Skip previous pages based on PageNumber
                .Take(dto.PageSize)  // Take the current page's records
                .ToListAsync();

            double phoneAmount = usages.Where(u => u.UsageType == "phone").Sum(u => u.Amount);
            double internetAmount = usages.Where(u => u.UsageType == "internet").Sum(u => u.Amount);


            return $"Phone Usage: {phoneAmount} minutes, Phone Charge: {CalculatePhoneCharge(phoneAmount)} USD\n" +
                   $"Internet Usage: {internetAmount} MB, Internet Charge: {CalculateInternetCharge(internetAmount)} USD\n" +
                   $"Total Amount: {bill.TotalAmount} USD, Paid Status: {bill.PaidStatus}";
        }

        public async Task<string> PayBill(PayBillDTO dto)
        {
            var bill = await _context.Bills
                           .Where(b => b.SubscriberNo == dto.SubscriberNo && b.Month == dto.Month && b.Year == dto.Year)
                           .FirstOrDefaultAsync();

            if (bill == null) return "Bill not found";

            bill.PaidStatus = "Paid";  // PaidStatus'ı string olarak "Paid" olarak güncelliyoruz
            await _context.SaveChangesAsync();

            return "Payment successful";
        }
    }
}
