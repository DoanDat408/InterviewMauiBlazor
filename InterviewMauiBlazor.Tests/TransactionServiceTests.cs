using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InterviewMauiBlazor.Database;
using InterviewMauiBlazor.Database.Entities;
using InterviewMauiBlazor.Services;
using InterviewMauiBlazor.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InterviewMauiBlazor.Tests
{
    public static class TestDbContextHelper
    {
        public static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            DatabaseSeeder.Seed(context);
            return context;
        }
    }

    public class TransactionServiceTests
    {
        [Fact]
        public async Task AddTransaction_Should_Add_New_Transaction()
        {
            using var context = TestDbContextHelper.GetInMemoryDbContext();
            var service = new TransactionService(context);

            var order = context.Orders.First();
            var product = context.Products.First();

            var transaction = new Transaction
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = 1,
                TotalPrice = product.Price, 
                Buyer = "Test Buyer",
                Seller = "Test Seller",
                Time = new DateTime(2025, 1, 10),
                Status = "Completed"
            };

            await service.AddTransactionAsync(transaction);

            var transactions = await service.GetTransactionsAsync();
            Assert.Contains(transactions, t =>
                t.Buyer == "Test Buyer" &&
                t.Time == new DateTime(2025, 1, 10) &&
                t.ProductId == product.Id &&
                t.OrderId == order.Id);
        }

        [Fact]
        public async Task UpdateTransaction_Should_Modify_Existing_Transaction()
        {
            using var context = TestDbContextHelper.GetInMemoryDbContext();
            var service = new TransactionService(context);

            var transaction = context.Transactions.First();
            int originalQuantity = transaction.Quantity;
            transaction.Quantity = originalQuantity + 1;

            await service.UpdateTransactionAsync(transaction);

            var updatedTransaction = await service.GetTransactionAsync(transaction.OrderId, transaction.ProductId, transaction.Time);
            Assert.Equal(originalQuantity + 1, updatedTransaction.Quantity);
        }

        [Fact]
        public async Task DeleteTransaction_Should_Remove_Transaction()
        {
            using var context = TestDbContextHelper.GetInMemoryDbContext();
            var service = new TransactionService(context);
            var transaction = context.Transactions.First();

            await service.DeleteTransactionAsync(transaction.OrderId, transaction.ProductId, transaction.Time);

            var transactions = await service.GetTransactionsAsync();
            Assert.DoesNotContain(transactions, t =>
                t.OrderId == transaction.OrderId &&
                t.ProductId == transaction.ProductId &&
                t.Time == transaction.Time);
        }

        [Fact]
        public async Task GenerateReport_Should_Return_Correct_Data()
        {
            using var context = TestDbContextHelper.GetInMemoryDbContext();
            var service = new TransactionService(context);
            var startDate = new DateTime(2025, 1, 1);
            var endDate = new DateTime(2025, 1, 5);

            var report = await service.GenerateReportAsync(startDate, endDate);

            Assert.True(report.TotalTransactions > 0);
            Assert.True(report.TotalValue > 0);
            Assert.NotNull(report.TransactionsByDay);
            Assert.NotNull(report.TopProducts);
        }

        [Fact]
        public void Transaction_EmptyBuyer_ShouldBeInvalid()
        {
            var viewModel = new TransactionViewModel
            {
                ProductId = 1,
                Quantity = 1,
                TotalPrice = 100,
                Buyer = "", // empty string
                Seller = "Test Seller",
                Time = DateTime.Now,
                Status = "Completed"
            };

            var context = new ValidationContext(viewModel);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(viewModel, context, results, validateAllProperties: true);

            foreach (var r in results)
            {
                Console.WriteLine(r.ErrorMessage);
            }

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Buyer is required"));
        }      
    }
}

