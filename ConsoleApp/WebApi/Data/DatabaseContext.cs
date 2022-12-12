using Common;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)    
        {

        }
        public DbSet<Person> Persons { get; set; }
    }
}
