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
        public virtual DbSet<ADMINISTRATOR> ADMINISTRATOR { get; set; }
        public virtual DbSet<BUYER> BUYERS { get; set; }
        public virtual DbSet<BUYER_PRODUCT_BOOKMARK> BUYER_PRODUCT_BOOKMARK { get; set; }
        public virtual DbSet<BUYER_STORE_BOOKMARK> BUYER_STORE_BOOKMARK { get; set; }
        public virtual DbSet<COMMENT_POST> COMMENT_POST { get; set; }
        public virtual DbSet<COMPLAIN_POST> COMPLAIN_POST { get; set; }
        public virtual DbSet<LIKE_POST> LIKE_POST { get; set; }
        public virtual DbSet<MARKET> MARKET { get; set; }
        public virtual DbSet<MARKET_PRODUCT> MARKET_PRODUCT { get; set; }
        public virtual DbSet<MARKET_STORE> MARKET_STORE { get; set; }
        public virtual DbSet<ORDERS> ORDERS { get; set; }
        public virtual DbSet<POST> POST { get; set; }
        public virtual DbSet<PRODUCT> PRODUCT { get; set; }
        public virtual DbSet<REPORT> REPORT { get; set; }
        public virtual DbSet<RETURN> RETURN { get; set; }
        public virtual DbSet<SET_UP_MARKETPLACE> SET_UP_MARKETPLACE { get; set; }
        public virtual DbSet<STORE> STORE { get; set; }
        public virtual DbSet<SUBMIT_AUTHENTICATION> SUBMIT_AUTHENTICATION { get; set; }
        public virtual DbSet<WALLET> WALLET { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<COMMENT_POST>()
                .HasKey(a => new { a.BUYER_ACCOUNT_ID, a.POST_ID, a.EVALUATION_TIME });

            modelBuilder.Entity<COMPLAIN_POST>()
                .HasKey(a => new { a.BUYER_ACCOUNT_ID, a.POST_ID, a.REPORT_ID });

            modelBuilder.Entity<LIKE_POST>()
                .HasKey(a => new { a.BUYER_ACCOUNT_ID, a.POST_ID });

            modelBuilder.Entity<MARKET_PRODUCT>()
                .HasKey(a => new { a.MARKET_ID, a.PRODUCT_ID });

            modelBuilder.Entity<SET_UP_MARKETPLACE>()
                .HasKey(a => new { a.MARKET_ID, a.ADMINISTRATOR_ACCOUNT_ID });

            modelBuilder.Entity<SUBMIT_AUTHENTICATION>()
                .HasKey(a => new { a.STORE_ACCOUNT_ID, a.ADMINISTRATOR_ACCOUNT_ID });

            modelBuilder.Entity<BUYER_STORE_BOOKMARK>()
                .HasKey(a => new { a.STORE_ACCOUNT_ID, a.BUYER_ACCOUNT_ID });

            modelBuilder.Entity<BUYER_PRODUCT_BOOKMARK>()
                .HasKey(a => new { a.PRODUCT_ID, a.BUYER_ACCOUNT_ID });

        }
    }
}
