using Microsoft.EntityFrameworkCore;
using BackendCode.Models;

namespace BackendCode.Data
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
