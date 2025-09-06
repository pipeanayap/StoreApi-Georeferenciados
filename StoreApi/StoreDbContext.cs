    using Microsoft.EntityFrameworkCore;
using StoreApi.Models.Entities;

namespace StoreApi;

public class StoreDbContext : DbContext
{
    public DbSet<Order> Order { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Store> Store { get; set; }
    public DbSet<SystemUser> SystemUser { get; set; }
    
    public DbSet<Invoice> Invoice { get; set; }

    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderProduct>().HasKey(p => new { p.OrderId, p.ProductId });

        modelBuilder.Entity<SystemUser>().HasData(
            new SystemUser
            {
                Id = 1,
                FirstName = "Paips",
                LastName = "Anaya",
                Email = "pipeanayap@gmail.com",
                Password = "pipeanayap"

            }
        );
        
        modelBuilder.Entity<Store>().HasData(
            new Store { Id = 1, Description = "Plaza Mayor León", Latitude = 21.1540, Longitude = -101.6946 },
            new Store { Id = 2, Description = "Centro Max", Latitude = 21.0948, Longitude = -101.6417 },
            new Store { Id = 3, Description = "Plaza Galerías Las Torres", Latitude = 21.1211, Longitude = -101.6613 },
            new Store { Id = 4, Description = "Outlet Mulza", Latitude = 21.0459, Longitude = -101.5862 },
            new Store { Id = 5, Description = "La Gran Plaza León", Latitude = 21.1280, Longitude = -101.6827 },
            new Store { Id = 6, Description = "Altacia", Latitude = 21.1280, Longitude = -102 }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Zapatos de piel", Description = "Calzado típico de León", Price = 1200, StoreId = 4 },
            new Product { Id = 2, Name = "Bolsa de cuero", Description = "Artesanía local", Price = 950, StoreId = 4 },
            new Product { Id = 3, Name = "Hamburguesa doble", Description = "Comida rápida", Price = 120, StoreId = 1 },
            new Product { Id = 4, Name = "Pizza familiar", Description = "Especialidad italiana", Price = 220, StoreId = 2 },
            new Product { Id = 5, Name = "Café americano", Description = "Taza grande", Price = 45, StoreId = 3 },
            new Product { Id = 6, Name = "Refresco 600ml", Description = "Bebida refrescante", Price = 20, StoreId = 5 }
        );
        
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice {
                Id = 1,
                InvoiceNumber = "INV-1001",
                IssueDate = new DateTime(2024, 6, 1),
                DueDate = new DateTime(2024, 6, 30),
                Subtotal = 1000,
                Tax = 200,
                Total = 1200,
                Currency = "MXN",
                IsPaid = true,
                PaymentDate = new DateTime(2024, 6, 6),
                BillingName = "Juan Pérez",
                BillingAddress = "Calle Falsa 123",
                BillingEmail = "juan.perez@email.com",
                TaxId = "ABC123456789",
                OrderId = 1
            },
            new Invoice {
                Id = 2,
                InvoiceNumber = "INV-1002",
                IssueDate = new DateTime(2024, 6, 3),
                DueDate = new DateTime(2024, 7, 1),
                Subtotal = 800,
                Tax = 150,
                Total = 950,
                Currency = "MXN",
                IsPaid = false,
                PaymentDate = DateTime.MinValue,
                BillingName = "María López",
                BillingAddress = "Av. Principal 456",
                BillingEmail = "maria.lopez@email.com",
                TaxId = "DEF987654321",
                OrderId = 2
            },
            new Invoice {
                Id = 3,
                InvoiceNumber = "INV-1003",
                IssueDate = new DateTime(2024, 6, 6),
                DueDate = new DateTime(2024, 7, 5),
                Subtotal = 200,
                Tax = 20,
                Total = 220,
                Currency = "MXN",
                IsPaid = true,
                PaymentDate = new DateTime(2024, 6, 9),
                BillingName = "Carlos Ruiz",
                BillingAddress = "Blvd. Central 789",
                BillingEmail = "carlos.ruiz@email.com",
                TaxId = "GHI456123789",
                OrderId = 3
            }
        );

    }
}