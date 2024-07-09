using Microsoft.EntityFrameworkCore;
using WebAPI.Models;


namespace WebApi1.Data
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNT { get; set; }
    }
}
