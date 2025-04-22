using Microsoft.EntityFrameworkCore;
using MobileBillingSystem.Data;
using MobileBillingSystem.DTO;
using MobileBillingSystem.DTO.MobileBillingSystem.DTOs;
using MobileBillingSystem.Models;


namespace MobileBillingSystem.Service
{
    public class UsageService
    {
        private readonly YourDbContext _context;
        private readonly BillService _billService;

        public UsageService(YourDbContext context, BillService billService)
        {
            _context = context;
            _billService = billService;  // DI ile BillService'i atadık
        }



        public async Task<string> AddUsage(AddUsageDTO dto)
        {
            // Subscriber'ı kontrol et
            var subscriber = await _context.Subscribers
                                           .Where(s => s.SubscriberNo == dto.SubscriberNo)
                                           .FirstOrDefaultAsync();

            // Eğer Subscriber bulunmazsa, hata mesajı döndür
            if (subscriber == null)
            {
                return "There is no subscriber with this no.";
            }

            // Yeni Usage objesini DTO'dan al
            var usage = new Usage
            {
                SubscriberNo = dto.SubscriberNo,
                UsageType = dto.UsageType,
                Amount = dto.Amount,  // Amount burada decimal olmalı
                Month = dto.Month,
                Year = dto.Year
            };

            // Usage verisini veritabanına ekle
            _context.Usages.Add(usage);
            await _context.SaveChangesAsync();  // Veriyi kaydettikten sonra faturayı hesapla

            // Fatura hesapla
            await _billService.CalculateBill(new CalculateBillDTO
            {
                SubscriberNo = dto.SubscriberNo,
                Month = dto.Month,
                Year = dto.Year
            });

            return "Usage succesfully added.";
        }
    }
}
