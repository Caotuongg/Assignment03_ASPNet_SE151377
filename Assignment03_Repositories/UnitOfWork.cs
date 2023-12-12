using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<AspNetUser> _userRepository;

        private IProductRepository _productRepository;

        private ICategoryRepository _categoryRepository;

        private IOrderRepository _orderRepository;
        private IOrderDetailsRepository _orderDetailsRepository;

        

        private readonly eStore_AspNetContext context;
        public UnitOfWork(eStore_AspNetContext context)
        {
            if (this.context == null)
            {
                this.context = context;
            }
            _userRepository = new GenericRepository<AspNetUser>(context);
            _productRepository = new ProductRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _orderRepository = new OrderRepository(context);
            _orderDetailsRepository = new OrderDetailsRepository(context);
            
        }
        public IGenericRepository<AspNetUser> UserRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                return _orderRepository;
            }
        }

        public IOrderDetailsRepository OrderDetailsRepository
        {
            get
            {
                return _orderDetailsRepository;
            }
        }



        public void SaveChange()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}

