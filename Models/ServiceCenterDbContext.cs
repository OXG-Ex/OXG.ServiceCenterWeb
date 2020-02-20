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
    }
}
