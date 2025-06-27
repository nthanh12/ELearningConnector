using Microsoft.EntityFrameworkCore;
using ElearningConnector.Models.Entities;

namespace ElearningConnector.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<DMBoPhan> DMBoPhans { get; set; } = null!;
        public DbSet<DMChucDanh> DMChucDanhs { get; set; } = null!;
        public DbSet<NhanVien> NhanViens { get; set; } = null!;
        public DbSet<HSNV_LichSuNhanVien> HSNV_LichSuNhanViens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<DMBoPhan>().ToTable("DMBoPhan");
            modelBuilder.Entity<DMChucDanh>().ToTable("DMChucDanh");
            modelBuilder.Entity<NhanVien>().ToTable("NhanVien");
            modelBuilder.Entity<HSNV_LichSuNhanVien>().ToTable("HSNV_LichSuNhanVien");
            base.OnModelCreating(modelBuilder);
        }
    }
}
