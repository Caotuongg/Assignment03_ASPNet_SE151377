using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<Category> GetCategory(int id)
        {
            try
            {
                var check = await categoryRepository.Get(id);
                if (check != null)
                {
                    return check;
                }
                else
                {
                    throw new Exception("Not Found Category");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                return categoryRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        


    }
}

