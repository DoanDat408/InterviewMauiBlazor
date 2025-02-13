using InterviewMauiBlazor.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewMauiBlazor.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactionsAsync();
        Task<Transaction> GetTransactionAsync(int orderId, int productId, DateTime time);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int orderId, int productId, DateTime time);
        Task<List<Transaction>> SearchTransactionsAsync(string searchText);
        Task<ReportData> GenerateReportAsync(DateTime startDate, DateTime endDate);
    }
}
