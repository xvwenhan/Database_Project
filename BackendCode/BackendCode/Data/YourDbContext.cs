using Microsoft.EntityFrameworkCore;
using BackendCode.Models;
using System;

namespace BackendCode.Data
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTS { get; set; }
        public virtual DbSet<ADMINISTRATOR> ADMINISTRATORS { get; set; }
        public virtual DbSet<BUYER> BUYERS { get; set; }
        public virtual DbSet<BUYER_PRODUCT_BOOKMARK> BUYER_PRODUCT_BOOKMARKS { get; set; }
        public virtual DbSet<BUYER_STORE_BOOKMARK> BUYER_STORE_BOOKMARKS { get; set; }
        public virtual DbSet<COMMENT_POST> COMMENT_POSTS { get; set; }
        public virtual DbSet<COMPLAIN_POST> COMPLAIN_POSTS { get; set; }
        public virtual DbSet<LIKE_POST> LIKE_POSTS { get; set; }
        public virtual DbSet<MARKET> MARKETS { get; set; }
        public virtual DbSet<MARKET_PRODUCT> MARKET_PRODUCTS { get; set; }
        public virtual DbSet<MARKET_STORE> MARKET_STORES { get; set; }
        public virtual DbSet<ORDERS> ORDERS { get; set; }
        public virtual DbSet<POST> POSTS { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTS { get; set; }
        public virtual DbSet<REPORT> REPORTS { get; set; }
        public virtual DbSet<RETURN> RETURNS { get; set; }
        public virtual DbSet<SET_UP_MARKETPLACE> SET_UP_MARKETPLACES { get; set; }
        public virtual DbSet<STORE> STORES { get; set; }
        public virtual DbSet<SUBMIT_AUTHENTICATION> SUBMIT_AUTHENTICATIONS { get; set; }
        public virtual DbSet<WALLET> WALLETS { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// 配置TPT 表继承策略
            modelBuilder.Entity<ACCOUNT>(entity =>
            {
                entity.ToTable("ACCOUNT");
                entity.HasKey(e => e.ACCOUNT_ID);

                entity.Property(e => e.ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)");
                entity.Property(e => e.USER_NAME)
                       .HasMaxLength(20)
                       .HasColumnType("VARCHAR2(20)");
                entity.Property(e => e.PHONE_NUMBER)
                       .HasMaxLength(11)
                       .HasColumnType("CHAR(11)");
                entity.Property(e => e.PASSWORD)
                       .HasMaxLength(15)
                       .HasColumnType("VARCHAR2(15)")
                       .IsRequired();
            });

            modelBuilder.Entity<ADMINISTRATOR>(entity =>
            {
                entity.ToTable("ADMINISTRATOR");
                //entity.HasKey(e => e.ACCOUNT_ID);
                entity.Property(e => e.PERMISSION_LEVEL)
                       .HasColumnType("INT");
                entity.HasOne<ACCOUNT>()
                       .WithOne()
                       .HasForeignKey<ADMINISTRATOR>(e => e.ACCOUNT_ID)
                       .HasConstraintName("ADMINISTRATOR_FK");
            });

            modelBuilder.Entity<BUYER>(entity =>
            {
                entity.ToTable("BUYER");
                //entity.HasKey(e => e.ACCOUNT_ID);

                entity.Property(e => e.GENDER)
                      .HasMaxLength(10)
                      .HasColumnType("VARCHAR2(10)");

                entity.Property(e => e.AGE)
                      .HasColumnType("NUMBER(3)");

                entity.Property(e => e.TOTAL_CREDITS)
                      .HasColumnType("NUMBER(4)")
                      .IsRequired();

                entity.Property(e => e.ADDRESS)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.HasOne<ACCOUNT>()
                      .WithOne()
                      .HasForeignKey<BUYER>(e => e.ACCOUNT_ID)
                      .HasConstraintName("BUYER_FK");
            });

            modelBuilder.Entity<STORE>(entity =>
            {
                entity.ToTable("STORE");
                //entity.HasKey(e => e.ACCOUNT_ID);

                entity.Property(e => e.ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.STORE_NAME)
                      .HasMaxLength(20)
                      .HasColumnType("VARCHAR2(20)")
                      .IsRequired();

                entity.Property(e => e.STORE_SCORE)
                      .HasColumnType("NUMBER(2,1)")
                      .IsRequired();

                entity.Property(e => e.CERTIFICATION)
                      .HasMaxLength(12)
                      .HasColumnType("VARCHAR2(12)")
                      .IsRequired();

                entity.Property(e => e.ADDRESS)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.HasOne<ACCOUNT>()
                      .WithOne()
                      .HasForeignKey<STORE>(e => e.ACCOUNT_ID)
                      .HasConstraintName("A_A_FK1");
            });


            modelBuilder.Entity<BUYER_PRODUCT_BOOKMARK>(entity =>
            {
                entity.ToTable("BUYER_PRODUCT_BOOKMARK");

                entity.HasKey(e => new { e.BUYER_ACCOUNT_ID, e.PRODUCT_ID });

                entity.Property(e => e.BUYER_ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.PRODUCT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.HasOne<BUYER>()
                      .WithMany()
                      .HasForeignKey(e => e.BUYER_ACCOUNT_ID)
                      .HasConstraintName("B_B_FK4");

                entity.HasOne<PRODUCT>()
                      .WithMany()
                      .HasForeignKey(e => e.PRODUCT_ID)
                      .HasConstraintName("P_P_FK3");
            });


            modelBuilder.Entity<BUYER_STORE_BOOKMARK>(entity =>
            {
                entity.ToTable("BUYER_STORE_BOOKMARK");

                entity.HasKey(e => new { e.BUYER_ACCOUNT_ID, e.STORE_ACCOUNT_ID });

                entity.Property(e => e.BUYER_ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.STORE_ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.HasOne<BUYER>()
                      .WithMany()
                      .HasForeignKey(e => e.BUYER_ACCOUNT_ID)
                      .HasConstraintName("B_B_FK");

                entity.HasOne<STORE>()
                      .WithMany()
                      .HasForeignKey(e => e.STORE_ACCOUNT_ID)
                      .HasConstraintName("S_S_FK2");
            });

            modelBuilder.Entity<COMMENT_POST>(entity =>
            {
                entity.ToTable("COMMENT_POST");

                entity.HasKey(e => new { e.BUYER_ACCOUNT_ID, e.POST_ID, e.EVALUATION_TIME });

                entity.Property(e => e.BUYER_ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.POST_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.EVALUATION_TIME)
                      .HasColumnType("TIMESTAMP(6)")
                      .IsRequired();

                entity.Property(e => e.EVALUATION_COMTENT)
                      .HasMaxLength(200)
                      .HasColumnType("VARCHAR2(200)");

                entity.HasOne<BUYER>()
                      .WithMany()
                      .HasForeignKey(e => e.BUYER_ACCOUNT_ID)
                      .HasConstraintName("B_B_FK1");

                entity.HasOne<POST>()
                      .WithMany()
                      .HasForeignKey(e => e.POST_ID)
                      .HasConstraintName("P_P_FK1");
            });

            modelBuilder.Entity<COMPLAIN_POST>(entity =>
            {
                entity.ToTable("COMPLAIN_POST");

                entity.HasKey(e => new { e.BUYER_ACCOUNT_ID, e.POST_ID, e.REPORT_ID });

                entity.Property(e => e.BUYER_ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.POST_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.REPORT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.HasOne<BUYER>()
                      .WithMany()
                      .HasForeignKey(e => e.BUYER_ACCOUNT_ID)
                      .HasConstraintName("B_B_FK3");

                entity.HasOne<POST>()
                      .WithMany()
                      .HasForeignKey(e => e.POST_ID)
                      .HasConstraintName("P_P_FK_3");

                entity.HasOne<REPORT>()
                      .WithMany()
                      .HasForeignKey(e => e.REPORT_ID)
                      .HasConstraintName("R_R_FK");
            });

            modelBuilder.Entity<LIKE_POST>(entity =>
            {
                entity.ToTable("LIKE_POST");

                entity.HasKey(e => new { e.BUYER_ACCOUNT_ID, e.POST_ID });

                entity.Property(e => e.BUYER_ACCOUNT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.POST_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.HasOne<BUYER>()
                      .WithMany()
                      .HasForeignKey(e => e.BUYER_ACCOUNT_ID)
                      .HasConstraintName("B_B_FK2");

                entity.HasOne<POST>()
                      .WithMany()
                      .HasForeignKey(e => e.POST_ID)
                      .HasConstraintName("P_P_FK2");
            });

            modelBuilder.Entity<MARKET>(entity =>
            {
                entity.ToTable("MARKET");

                entity.HasKey(e => e.MARKET_ID);

                entity.Property(e => e.MARKET_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.THEME)
                      .HasMaxLength(50)
                      .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.START_TIME)
                      .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.END_TIME)
                      .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DISCOUNT_REQUIREMENT)
                      .HasMaxLength(50)
                      .HasColumnType("VARCHAR2(50)");
            });

            modelBuilder.Entity<MARKET_PRODUCT>(entity =>
            {
                entity.ToTable("MARKET_PRODUCT");

                entity.HasKey(e => new { e.MARKET_ID, e.PRODUCT_ID });

                entity.Property(e => e.MARKET_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.PRODUCT_ID)
                      .HasMaxLength(100)
                      .HasColumnType("VARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.DISCOUNT_PRICE)
                      .HasColumnType("NUMBER(7,2)");

                entity.HasOne<MARKET>()
                      .WithMany()
                      .HasForeignKey(e => e.MARKET_ID)
                      .HasConstraintName("M_M_FK");

                entity.HasOne<PRODUCT>()
                      .WithMany()
                      .HasForeignKey(e => e.PRODUCT_ID)
                      .HasConstraintName("P_P_FK");
            });


        }
    }
}
