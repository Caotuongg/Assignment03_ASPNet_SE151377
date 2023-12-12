using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IProductRepository productRepository )
        {
            this.unitOfWork = unitOfWork;
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                var check = await unitOfWork.ProductRepository.Get(id);
                if (check != null)
                {
                    check.Category = await unitOfWork.CategoryRepository.Get(check.CategoryId);
                    return check;
                }
                else
                {
                    throw new Exception("Not Found Product");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Product>> GetAll(string search)
        {
            try
            {
                var products = new List<Product>();
                if (string.IsNullOrWhiteSpace(search))
                {
                    var uow = await unitOfWork.ProductRepository.GetAllProduct();
                    products = uow.ToList();
                }
                else
                {
                    var repo = await unitOfWork.ProductRepository.GetAllProducts(search.Trim());
                    products = repo.ToList();
                }
                foreach (var product in products)
                {
                    product.Category = await categoryRepository.Get(product.CategoryId);

                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Add(Product product)
        {
            try
            {
                return await unitOfWork.ProductRepository.Add(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(Product product)
        {
            try
            {
                var check = await unitOfWork.ProductRepository.Get(product.ProductId);
                if (check != null)
                {
                    check.ProductName = product.ProductName;
                    check.Weight = product.Weight;
                    check.UnitPrice = product.UnitPrice;
                    check.UnitsInStock = product.UnitsInStock;
                    
                    return await unitOfWork.ProductRepository.Update(product.ProductId, check);

                }
                else
                {
                    throw new Exception("Not Found Product");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var mem = await unitOfWork.ProductRepository.Get(id);
                if (mem != null)
                {
                    return await unitOfWork.ProductRepository.Delete(id);

                }
                else
                {
                    throw new Exception("Not Found Product");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
