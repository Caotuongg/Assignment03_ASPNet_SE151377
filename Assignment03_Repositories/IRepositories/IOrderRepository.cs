using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories.IRepositories
{
    public interface IOrderRepository: IGenericRepository<Order>
    {
        IEnumerable<Order> GetReport(DateTime startDate, DateTime endDate);
        Order GetLastOrder(string memId);

    }
}
