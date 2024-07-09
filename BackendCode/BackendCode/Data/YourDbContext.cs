using Microsoft.EntityFrameworkCore;
using BackendCode.Models;


namespace BackendData.Data
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
