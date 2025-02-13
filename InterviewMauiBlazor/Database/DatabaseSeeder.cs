using System;
using System.Collections.Generic;
using System.Linq;
using InterviewMauiBlazor.Database.Entities;

namespace InterviewMauiBlazor.Database
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer { Name = "John Doe", Email = "john.doe@example.com" },
                    new Customer { Name = "Jane Smith", Email = "jane.smith@example.com" }
                };
                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                var customerJohn = context.Customers.FirstOrDefault(c => c.Name == "John Doe");
                var customerJane = context.Customers.FirstOrDefault(c => c.Name == "Jane Smith");

                var orders = new List<Order>
                {
                    new Order { OrderDate = new DateTime(2025, 1, 1), Customer = customerJohn, CustomerId = customerJohn.Id },
                    new Order { OrderDate = new DateTime(2025, 1, 5), Customer = customerJane, CustomerId = customerJane.Id }
                };
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Laptop", Price = 999.99m },
                    new Product { Name = "Phone", Price = 699.99m },
                    new Product { Name = "Tablet", Price = 499.50m }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            if (!context.Transactions.Any())
            {
                var order1 = context.Orders.FirstOrDefault(o => o.OrderDate == new DateTime(2025, 1, 1));
                var order2 = context.Orders.FirstOrDefault(o => o.OrderDate == new DateTime(2025, 1, 5));
                var laptop = context.Products.FirstOrDefault(p => p.Name == "Laptop");
                var phone = context.Products.FirstOrDefault(p => p.Name == "Phone");

                if (order1 != null && laptop != null)
                {
                    var transaction1 = new Transaction
                    {
                        OrderId = order1.Id,
                        Order = order1,
                        ProductId = laptop.Id,
                        Product = laptop,
                        Quantity = 1,
                        TotalPrice = 999.99m,
                        Time = new DateTime(2025, 1, 1),
                        Buyer = "John Doe",
                        Seller = "Store A",
                        Status = "Completed"
                    };
                    context.Transactions.Add(transaction1);
                }

                if (order2 != null && phone != null)
                {
                    var transaction2 = new Transaction
                    {
                        OrderId = order2.Id,
                        Order = order2,
                        ProductId = phone.Id,
                        Product = phone,
                        Quantity = 2,
                        TotalPrice = 1399.98m,
                        Time = new DateTime(2025, 1, 5),
                        Buyer = "Jane Smith",
                        Seller = "Store B",
                        Status = "Pending"
                    };
                    context.Transactions.Add(transaction2);
                }

                context.SaveChanges();
            }
        }
    }
}
