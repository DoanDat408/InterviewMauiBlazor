using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewMauiBlazor.Services
{
    public class ReportData
    {
        public int TotalTransactions { get; set; }
        public decimal TotalValue { get; set; }
        public Dictionary<DateTime, int> TransactionsByDay { get; set; }
        public List<TopProduct> TopProducts { get; set; }
    }

    public class TopProduct
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
    }
}
