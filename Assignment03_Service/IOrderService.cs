using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public interface IOrderService
    {
        Task<bool> AddProduct(string customerId, int productId);
        Order GetCart(string customerId);
        Task<bool> BuyCart(string customerId);
        Order GetOrderById(string customerId, int orderId);
        Order GetOrderDetail(int orderId);
        IEnumerable<Order> GetOrders(DateTime? startDate, DateTime? endDate);
        IEnumerable<Order> GetOrdersOfCus(string customerId);
        Task<bool> RemoveCart(string customerId);
        Task<bool> RemoveProduct(string customerId, int productId);
        int TotalProductOfOrder(int orderId);
        decimal TotalPriceOfOrder(int orderId);
    }
}
