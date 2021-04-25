using Microsoft.EntityFrameworkCore;
using BL.Models;
using BL.Config;

namespace DAL
{
    public class BillsContext : DbContext
    {
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Users> Users { get; set; }

        public BillsContext(DbContextOptions<BillsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new OrderDetailsConfig())
                .ApplyConfiguration(new OrdersConfig())
                .ApplyConfiguration(new UsersCofig());
        }


    }
}
