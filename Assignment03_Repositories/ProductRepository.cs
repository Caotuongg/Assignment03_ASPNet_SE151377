using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories
{
    public class ProductRepository: GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(eStore_AspNetContext eStore_AspNetContext) : base(eStore_AspNetContext) 
        { 
        
        }

        public async Task<IEnumerable<Product>> GetAllProducts(string search)
        {
            try
            {
                if (decimal.TryParse(search, out var result))
                {
                    return await dbSet.Where(x => x.UnitPrice == result).ToListAsync();
                }
                else
                {
                    return await dbSet.Where(x => x.ProductName.ToLower().Contains(search.ToLower())).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdateRange(List<Product> products)
        {
            try
            {
                dbSet.UpdateRange(products);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

   
}
