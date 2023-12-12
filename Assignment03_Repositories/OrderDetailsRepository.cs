using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories
{
    public class OrderDetailsRepository: GenericRepository<OrderDetail>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(eStore_AspNetContext dBContext) : base(dBContext)
        {
        }

        public IEnumerable<OrderDetail> GetAll(int id)
        {
            try
            {
                return dbSet.Where(x => x.OrderId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void RemoveRange(int orderId)
        {
            try
            {
                var orderDetails = dbSet.Where(x => x.OrderId == orderId).ToList();
                dbSet.RemoveRange(orderDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
