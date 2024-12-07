using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Migrations;

namespace Backend.DataConnection
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Tour> Tour { get; set; }
    }

}
