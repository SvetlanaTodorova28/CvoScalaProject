using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Web.Data.Seeding;

namespace Scala.StockSimulation.Web.Data
{
    public class ScalaStockSimulationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserProductState> UserProductStates { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        
		
		
        public ScalaStockSimulationDbContext(DbContextOptions<ScalaStockSimulationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ApplicationUser>()
                .Property(a => a.FirstName)
                .HasMaxLength(100);

            modelBuilder.Entity<ApplicationUser>()
                .Property(a => a.LastName)
                .HasMaxLength(100);


            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("money");


            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderTypeId)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.ApplicationUserId)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Id)
                .IsRequired();
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.OrderId)
                .IsRequired();
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.ProductId)
                .IsRequired();

            modelBuilder.Entity<UserProductState>()
                .Property(u => u.Id)
                .IsRequired();
            modelBuilder.Entity<UserProductState>()
                .Property(u => u.ApplicationUserId)
                .IsRequired();
            modelBuilder.Entity<UserProductState>()
                .Property(u => u.ProductId)
                .IsRequired();

            Seeder.Seed(modelBuilder);

        }
    }
}
