using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Data
{
    public class ApplicationDbContextConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            // Add any additional entity configurations here
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Add any seed data here

            // Generate fake customer data.
            var customerIds = Enumerable.Range(1, 100).ToList();
            var fakerSeedCustomers = new Faker<Customer>()
                .RuleFor(c => c.Id, f =>
                {
                    // Pop the next unique Id from the list
                    var index = f.Random.Int(0, customerIds.Count() - 1);
                    var id = customerIds[index];
                    customerIds.RemoveAt(index);
                    return id;
                })
                .RuleFor(c => c.FullName, f => f.Name.FullName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Balance, f => f.Random.Decimal(1, 10000).OrNull(f));

            var fakeCustomers = fakerSeedCustomers.Generate(100);
            modelBuilder.Entity<Customer>().HasData(fakeCustomers);

            // Generate fake product data.
            var productIds = Enumerable.Range(1, 100).ToList();
            var fakerSeedProducts = new Faker<Product>()
                .RuleFor(c => c.Id, f =>
                {
                    var index = f.Random.Int(0, productIds.Count() - 1);
                    var id = productIds[index];
                    productIds.RemoveAt(index);
                    return id;
                })
                .RuleFor(p => p.Code, f => f.Random.AlphaNumeric(6))
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => f.Random.Double(1, 1000))
                .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(p => p.IsActive, f => f.Random.Bool());

            var fakeProducts = fakerSeedProducts.Generate(100);
            modelBuilder.Entity<Product>().HasData(fakeProducts);


            // Generate fake order data.
            var orderIds = Enumerable.Range(1, 1000).ToList();

            var fakerSeedOrders = new Faker<Order>()
                .RuleFor(c => c.Id, f =>
                {
                    var index = f.Random.Int(0, orderIds.Count() - 1);
                    var id = orderIds[index];
                    orderIds.RemoveAt(index);
                    return id;
                })
                .RuleFor(o => o.CustomerId, f => f.Random.Int(1, 100))
                .RuleFor(o => o.TotalBill, f => f.Random.Decimal(10, 10000))
                .RuleFor(o => o.TotalQuantity, f => f.Random.Int(1, 1000))
                .RuleFor(o => o.ProcessingData, f => f.Date.Past());

            var fakeOrders = fakerSeedOrders.Generate(1000);
            modelBuilder.Entity<Order>().HasData(fakeOrders);

            // Generate fake order details data.
            var orderDetailsIds = Enumerable.Range(1, 3000).ToList();

            var fakerSeedOrderDetails = new Faker<OrderDetails>()
                .RuleFor(c => c.Id, f =>
                {
                    var index = f.Random.Int(0, orderDetailsIds.Count() - 1);
                    var id = orderDetailsIds[index];
                    orderDetailsIds.RemoveAt(index);
                    return id;
                })
                .RuleFor(od => od.OrderId, f => f.Random.Int(1, 1000))
                .RuleFor(od => od.ProductId, f => f.Random.Int(1, 100))
                .RuleFor(od => od.Quantity, f => f.Random.Int(1, 1000))
                .RuleFor(od => od.SellingPrice, f => f.Random.Decimal(1, 1000));

            var fakeOrderDetails = fakerSeedOrderDetails.Generate(3000);
            modelBuilder.Entity<OrderDetails>().HasData(fakeOrderDetails);


        }

    }
}
