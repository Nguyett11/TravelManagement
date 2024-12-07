using Microsoft.EntityFrameworkCore;
using Reporting.Models;

namespace Reporting.DataConetion
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
        public DbSet<Reporting.Models.orders_reports> orders_reports { get; set; } = default!;
        public DbSet<Reporting.Models.product_reports> product_reports { get; set; } = default!;
        public DbSet<orders_reports> ordersReports { get; set; }
        public DbSet<product_reports> productReports { get; set; }
    }
}
