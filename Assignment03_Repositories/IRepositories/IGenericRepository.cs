using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> Get(object id);
        Task<bool> Add(T item);
        Task<bool> Update(object id, T item);
        Task<bool> Delete(object id);

        Task<IEnumerable<T>> GetAllProduct();
        IEnumerable<T> GetAllOrder(Func<T, bool> func);
        Task<bool> UpdateOrder(object id1, object id2, T item);
        Task<bool> UpdateProducts(List<Product> products);
        Task<bool> DeleteAll(List<object> id1s, object id2);
        Task<bool> DeleteOrder(object id1, object id2);
    }
}
