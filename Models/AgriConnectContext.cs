using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AgriEnergyWebApp.Models
{
    public partial class AgriConnectContext : DbContext
    {
        public AgriConnectContext()
        {
        }

        public AgriConnectContext(DbContextOptions<AgriConnectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Farmer> Farmers { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:appr-server.database.windows.net,1433;Initial Catalog=AgriConnectDB;Persist Security Info=False;User ID=apprAdmin;Password=Admin420;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK__Employee__59AF14B517A630CE");

                entity.ToTable("Employee");

                entity.Property(e => e.AdminId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADMIN_ID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");
            });

            modelBuilder.Entity<Farmer>(entity =>
            {
                entity.ToTable("Farmer");

                entity.Property(e => e.FarmerId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FARMER_ID");

                entity.Property(e => e.FarmerName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FARMER_NAME");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");

                entity.Property(e => e.FarmerId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FARMER_ID");

                entity.Property(e => e.ProductDate)
                    .HasColumnType("date")
                    .HasColumnName("PRODUCT_DATE");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_NAME");

                entity.Property(e => e.ProductPrice).HasColumnName("PRODUCT_PRICE");

                entity.Property(e => e.ProductQuantity).HasColumnName("PRODUCT_QUANTITY");

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCT_TYPE");

                entity.HasOne(d => d.Farmer)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.FarmerId)
                    .HasConstraintName("FK__Products__FARMER__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
