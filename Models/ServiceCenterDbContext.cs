using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class ServiceCenterDbContext : DbContext
    {
        public ServiceCenterDbContext(DbContextOptions<ServiceCenterDbContext> options) : base (options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Component> Components { get; set; }

        public DbSet<Employeer> Employeers { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<Work> Works { get; set; }

        //public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                    new Role {Id = 1, Name = "Администратор" },
                    new Role {Id = 2, Name = "Мастер" },
                    new Role {Id = 3, Name = "Приёмщик" }
                }
                );
            modelBuilder.Entity<Work>().HasData(
                new Work[]
                {
                    new Work(){Id=1, Name ="Настройка ОС", Num=1,Price=1200},
                    new Work(){Id=2, Name ="Чистка СО", Num=1,Price=450},
                    new Work(){Id=3, Name ="Оптимизация ОС", Num=1,Price=500},
                    new Work(){Id=4, Name ="Настройка ОС с сохр. данных", Num=1,Price=1500},
                    new Work(){Id=5, Name ="Диагностика", Num=1,Price=400},
                    new Work(){Id=6, Name ="Заправка картирджа HP, Canon черн. лазерн.", Num=1,Price=350},
                    new Work(){Id=7, Name ="Заправка картирджа Samsung черн. лазерн.", Num=1,Price=400},
                    new Work(){Id=8, Name ="Пересборка", Num=1,Price=450},
                    new Work(){Id=9, Name ="Замена блока питания", Num=1,Price=450},
                    new Work(){Id=10, Name ="Чистка СО ноутбука", Num=1,Price=1000}
                }
                );
            modelBuilder.Entity<Employeer>().HasData(
                new Employeer { Id = 1, Name = "Администратор", Email = "admin@admin.com", Password = "admin", RoleId = 1 }
                );
        }
    }
}
