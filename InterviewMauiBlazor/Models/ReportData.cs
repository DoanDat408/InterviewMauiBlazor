using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewMauiBlazor.Reports
{
    public class ReportData
    {
        public int TotalTransactions { get; set; }
        public decimal TotalValue { get; set; }
        public Dictionary<DateTime, int> TransactionsByDay { get; set; }
        public List<TopProduct> TopProducts { get; set; }
    }
}
