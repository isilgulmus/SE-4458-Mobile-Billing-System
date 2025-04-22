using Microsoft.EntityFrameworkCore;
using MobileBillingSystem.Models;  

namespace MobileBillingSystem.Data
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) { }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Usage> Usages { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscriber>()
                        .HasKey(s => s.SubscriberNo);

            modelBuilder.Entity<Bill>()
                        .HasOne(b => b.Subscriber)
                        .WithMany(s => s.Bills)
                        .HasForeignKey(b => b.SubscriberNo);

            modelBuilder.Entity<Usage>()
                        .HasOne(u => u.Subscriber)
                        .WithMany(s => s.Usages)
                        .HasForeignKey(u => u.SubscriberNo);


            modelBuilder.Entity<Bill>()
                        .Property(b => b.TotalAmount)
                        .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<Payment>()
                        .Property(p => p.AmountPaid)
                        .HasColumnType("decimal(18,2)"); 
        }
    }
}
