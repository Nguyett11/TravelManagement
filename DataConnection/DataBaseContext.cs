using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.DataConnection
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
}

}
