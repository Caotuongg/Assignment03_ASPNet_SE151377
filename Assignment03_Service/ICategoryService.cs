using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public interface ICategoryService
    {
        Task<Category> GetCategory(int id);
        IEnumerable<Category> GetAll();
        
    }
}
