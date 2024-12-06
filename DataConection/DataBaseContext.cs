using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TourManagement.Models;

namespace TourManagement.DataConection
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
        public DbSet<Tour> Tour { get; set; }
    }
}
