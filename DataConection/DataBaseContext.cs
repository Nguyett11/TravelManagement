using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Models;

namespace OrderManagement.DataConection
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
        public DbSet<Order>  Order { get; set; }
        public DbSet<orderDetail> OrderDetail { get; set; }
    }
}
