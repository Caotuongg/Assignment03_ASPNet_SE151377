using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories.IRepositories
{
    public interface IOrderDetailsRepository: IGenericRepository<OrderDetail>
    {
        IEnumerable<OrderDetail> GetAll(int id);
        void RemoveRange(int orderId);
    }
}
