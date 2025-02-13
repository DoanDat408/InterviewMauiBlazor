using InterviewMauiBlazor.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewMauiBlazor.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
    }
}
