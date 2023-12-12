using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories
{
    public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(eStore_AspNetContext eStore_AspNetContext): base(eStore_AspNetContext) { }
    }
}
