using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using InventorySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Models
{
    public partial class InventoryContext : DbContext
    {
        public InventoryContext()
        {
        }
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;database=mvc_productinventory", x => x.ServerVersion("10.4.14-mariadb"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");


            entity.HasData(
                 new Product()
                 {
                     ID = -1,
                     Name = "Chair",
                     Quantity = 5,
                     IsDiscontinued=false

                 },
                new Product()
                {
                    ID = -2,
                    Name = "Table",
                    Quantity = 3,
                    IsDiscontinued = false
                },
                new Product()
                {
                    ID = -3,
                    Name = "Pen",
                    Quantity = 0,
                    IsDiscontinued = false
                },
                new Product()
                {
                    ID = -4,
                    Name = "Pencil",
                    Quantity = 6,
                    IsDiscontinued = false

                },
                new Product()
                {
                    ID = -5,
                    Name = "Cardboard",
                    Quantity = 10,
                    IsDiscontinued = false
                },
                new Product()
                {
                    ID = -6,
                    Name = "Desk",
                    Quantity = 0,
                    IsDiscontinued = false
                }
                );

            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public DbSet<InventorySystem.Models.Product> Product { get; set; }
    }
}



