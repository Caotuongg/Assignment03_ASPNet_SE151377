using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public interface IProductService
    {
        Task<Product> GetProductById(int id);


        Task<bool> Add(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(int id);
        Task<IEnumerable<Product>> GetAll(string search);
    }
}
