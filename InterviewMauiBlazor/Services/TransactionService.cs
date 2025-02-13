using InterviewMauiBlazor.Database;
using InterviewMauiBlazor.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewMauiBlazor.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Order)
                    .ThenInclude(o => o.Customer)
                .Include(t => t.Product)
                .OrderByDescending(t => t.Time)
                .ToListAsync();
        }

        public async Task<Transaction> GetTransactionAsync(int orderId, int productId, DateTime time)
        {
            return await _context.Transactions
                .Include(t => t.Order)
                    .ThenInclude(o => o.Customer)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(t => t.OrderId == orderId
                                           && t.ProductId == productId
                                           && t.Time.Date == time.Date);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            var trackedEntity = await _context.Transactions.FindAsync(transaction.OrderId, transaction.ProductId, transaction.Time);
            if (trackedEntity != null)
            {
                trackedEntity.Quantity = transaction.Quantity;
                trackedEntity.TotalPrice = transaction.TotalPrice;
                trackedEntity.Buyer = transaction.Buyer;
                trackedEntity.Seller = transaction.Seller;
                trackedEntity.Status = transaction.Status;
            }
            else
            {
                _context.Attach(transaction);
                _context.Entry(transaction).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }


        public async Task DeleteTransactionAsync(int orderId, int productId, DateTime time)
        {
            var transaction = await GetTransactionAsync(orderId, productId, time);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Transaction>> SearchTransactionsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetTransactionsAsync();

            return await _context.Transactions
                .Include(t => t.Order)
                    .ThenInclude(o => o.Customer)
                .Include(t => t.Product)
                .Where(t =>
                        (t.Buyer != null && t.Buyer.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (t.Seller != null && t.Seller.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (t.Status != null && t.Status.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (t.Product.Name != null && t.Product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                      )
                .OrderByDescending(t => t.Time)
                .ToListAsync();
        }

        public async Task<ReportData> GenerateReportAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Time >= startDate && t.Time <= endDate)
                .Include(t => t.Product)
                .ToListAsync();

            var reportData = new ReportData
            {
                TotalTransactions = transactions.Count,
                TotalValue = transactions.Sum(t => t.TotalPrice),
                TransactionsByDay = transactions.GroupBy(t => t.Time.Date)
                                                .ToDictionary(g => g.Key, g => g.Count()),
                TopProducts = transactions.GroupBy(t => t.Product.Name)
                                          .Select(g => new TopProduct { ProductName = g.Key, Count = g.Count() })
                                          .OrderByDescending(tp => tp.Count)
                                          .ToList()
            };

            return reportData;
        }
    }
}
