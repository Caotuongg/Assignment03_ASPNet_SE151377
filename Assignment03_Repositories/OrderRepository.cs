using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories
{
    public class OrderRepository: GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(eStore_AspNetContext eStore_AspNetContext) : base(eStore_AspNetContext) 
        { 
        
        }

        public IEnumerable<Order> GetReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                return dbSet.Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order GetLastOrder(string memId)
        {
            try
            {
                return dbSet.Where(x => x.MemberId == memId).OrderByDescending(x => x.OrderId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


}
