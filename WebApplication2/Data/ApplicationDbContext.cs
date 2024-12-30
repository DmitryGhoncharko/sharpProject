using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Ваши DbSet для продуктов, корзины и заказов
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Устанавливаем максимальную длину для строковых полей, чтобы избежать ошибок с MySQL
            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.Id).HasMaxLength(128).HasColumnType("varchar(128)"); // Указание максимальной длины и типа данных
                entity.Property(r => r.Name).HasMaxLength(256).HasColumnType("varchar(256)"); // Максимальная длина для имени роли
                entity.HasIndex(r => r.NormalizedName).IsUnique(); // Уникальный индекс для NormalizedName
            });

            builder.Entity<IdentityUser>(entity =>
            {
                entity.Property(u => u.Id).HasMaxLength(128).HasColumnType("varchar(128)"); // Указание максимальной длины и типа данных
                entity.Property(u => u.UserName).HasMaxLength(256).HasColumnType("varchar(256)"); // Максимальная длина для имени пользователя
                entity.Property(u => u.NormalizedUserName).HasMaxLength(256).HasColumnType("varchar(256)"); // Максимальная длина для нормализованного имени
                entity.HasIndex(u => u.NormalizedUserName).IsUnique(); // Уникальный индекс для NormalizedUserName
            });
        }
    }
}