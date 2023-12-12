using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public class OrderService: IOrderService
    {
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly IOrderDetailsRepository orderDetailsRepository;
        public OrderService(IUserRepository userRepository, IOrderRepository orderRepository, IProductRepository productRepository, IOrderDetailsRepository orderDetailsRepository)
        {
            this.userRepository = userRepository;
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.orderDetailsRepository = orderDetailsRepository;
        }
        public async Task<bool> AddProduct(string customerId, int productId)
        {
            try
            {
                if (await userRepository.Get(customerId) == null)
                {
                    throw new Exception("Not Found User");
                }
                var cart = orderRepository.GetAllOrder(x => x.IsPayment == false && x.MemberId == customerId).FirstOrDefault();
                if (cart == null)
                {
                    cart = new Order
                    {
                        MemberId = customerId,
                        IsDeleted = false,
                        IsPayment = false,
                        OrderDate = DateTime.Now,
                        RequiredDate = DateTime.Now,
                        ShippedDate = DateTime.Now,
                        
                    };
                    if (await orderRepository.Add(cart))
                    {
                        var product = await productRepository.Get(productId);
                        if (product != null)
                        {
                            var orderDetail = new OrderDetail
                            {
                                Discount = 0,
                               
                                OrderId = cart.OrderId,
                                ProductId = productId,
                                Quantity = 1,
                                UnitPrice = product.UnitPrice,
                            };
                            if (await orderDetailsRepository.Add(orderDetail))
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("Add Fail");
                            }
                        }
                        else
                        {
                            throw new Exception("Not Found Product");
                        }
                    }
                    else
                    {
                        throw new Exception("Add Fail");
                    }
                }
                else
                {
                    var product = await productRepository.Get(productId);
                    if (product != null)
                    {
                        var orderDetail = orderDetailsRepository.GetAllOrder(x => x.OrderId == cart.OrderId && x.ProductId == productId).FirstOrDefault();
                        if (orderDetail != null)
                        {
                            orderDetail.Quantity += 1;
                            orderDetail.UnitPrice += product.UnitPrice;
                            
                            if (await orderDetailsRepository.UpdateOrder(orderDetail.OrderId, orderDetail.ProductId, orderDetail))
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("Add Fail");
                            }
                        }
                        else
                        {
                            if (await orderDetailsRepository.Add(new OrderDetail
                            {
                                Discount = 0,
                               
                                ProductId = product.ProductId,
                                OrderId = cart.OrderId,
                                Quantity = 1,
                                UnitPrice = product.UnitPrice,
                            }))
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("Add Fail");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Not Found Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order GetCart(string customerId)
        {
            try
            {
                if (userRepository.Get(customerId).Result == null)
                {
                    throw new Exception("Not Found User");
                }
                var order = orderRepository.GetAllOrder(x => x.IsDeleted == false && x.IsPayment == false && x.MemberId == customerId).FirstOrDefault();
                if (order != null)
                {
                    order.OrderDetails = new List<OrderDetail>();
                    order.OrderDetails = orderDetailsRepository.GetAllOrder(x => x.OrderId == order.OrderId).ToList();
                }
                else
                {
                    order = new Order
                    {
                        
                        IsDeleted = false,
                        IsPayment = false,
                        OrderDate = DateTime.Now,
                        RequiredDate = DateTime.Now,
                        ShippedDate = DateTime.Now,
                        MemberId = customerId,
                    };
                    if (orderRepository.Add(order).Result)
                    {
                        return order;
                    }
                    else
                    {
                        throw new Exception("Error When Create New Cart");
                    }
                }
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> BuyCart(string customerId)
        {
            try
            {
                if (userRepository.Get(customerId).Result == null)
                {
                    throw new Exception("Not Found User");
                }
                var order = orderRepository.GetAllOrder(x => x.IsDeleted == false && x.IsPayment == false && x.MemberId == customerId).FirstOrDefault();
                if (order != null)
                {
                    order.OrderDetails = orderDetailsRepository.GetAllOrder(x => x.OrderId == order.OrderId).ToList();
                    if (!order.OrderDetails.Any())
                    {
                        throw new Exception("Empty Cart");
                    }
                    var products = new List<Product>();
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        var product = await productRepository.Get(orderDetail.ProductId);
                        if (product.UnitsInStock == 0)
                        {
                            throw new Exception($"Product {product.ProductName} is out of stock");
                        }
                        else if (orderDetail.Quantity > product.UnitsInStock)
                        {
                            throw new Exception($"Product {product.ProductName} is greater than unit in stock");
                        }
                        else
                        {
                            product.UnitsInStock -= orderDetail.Quantity;
                            products.Add(product);
                        }
                    }
                    if (await productRepository.UpdateProducts(products))
                    {
                        order.ShippedDate = DateTime.Now;
                        order.RequiredDate = DateTime.Now;
                        order.OrderDate = DateTime.Now;
                        order.IsPayment = true;
                        if (await orderRepository.Update(order.OrderId, order))
                        {
                            return true;
                        }
                        else
                        {
                            throw new Exception("Buy Cart Fail");
                        }
                    }
                    else
                    {
                        throw new Exception("Buy Cart Fail");
                    }
                }
                else
                {
                    throw new Exception("Not Found Cart");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order GetOrderById(string customerId, int orderId)
        {
            try
            {
                if (userRepository.Get(customerId).Result == null)
                {
                    throw new Exception("Not Found User");
                }
                var order = orderRepository.GetAllOrder(x => x.IsDeleted == false && x.IsPayment == true && x.MemberId == customerId && x.OrderId == orderId).FirstOrDefault();
                if (order != null)
                {
                    order.OrderDetails = orderDetailsRepository.GetAllOrder(x => x.OrderId == orderId).ToList();
                }
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order GetOrderDetail(int orderId)
        {
            try
            {
                var order = orderRepository.GetAllOrder(x => x.IsDeleted == false && x.IsPayment == true && x.OrderId == orderId).FirstOrDefault();
                if (order != null)
                {
                    order.OrderDetails = orderDetailsRepository.GetAllOrder(x => x.OrderId == orderId).ToList();
                }
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Order> GetOrders(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                return orderRepository.GetAllOrder(x => x.IsDeleted == false && x.IsPayment == true
                && (!startDate.HasValue || (startDate.HasValue && x.OrderDate.Date >= startDate.Value.Date))
                && (!endDate.HasValue || (endDate.HasValue && x.OrderDate.Date <= endDate.Value.AddDays(1).AddMilliseconds(-1).Date))
                ).OrderByDescending(x => x.OrderDate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Order> GetOrdersOfCus(string customerId)
        {
            try
            {
                if (userRepository.Get(customerId).Result == null)
                {
                    throw new Exception("Not Found User");
                }
                return orderRepository.GetAllOrder(x => x.IsDeleted == false && x.IsPayment == true && x.MemberId == customerId).OrderByDescending(x => x.OrderDate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveCart(string customerId)
        {
            try
            {
                if (await userRepository.Get(customerId) == null)
                {
                    throw new Exception("Not Found User");
                }
                var cart = orderRepository.GetAllOrder(x => x.IsPayment == false && x.MemberId == customerId).FirstOrDefault();
                if (cart == null)
                {
                    throw new Exception("Not Found Cart");
                }
                var orderDetails = orderDetailsRepository.GetAllOrder(x => x.OrderId == cart.OrderId).ToList();
                if (orderDetails.Any())
                {
                    if (await orderDetailsRepository.DeleteAll(orderDetails.Select(x => (object)x.ProductId).ToList(), cart.OrderId))
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("Remove Fail");
                    }
                }
                else
                {
                    throw new Exception("Cart Empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveProduct(string customerId, int productId)
        {
            try
            {
                if (await userRepository.Get(customerId) == null)
                {
                    throw new Exception("Not Found User");
                }
                var cart = orderRepository.GetAllOrder(x => x.IsPayment == false && x.MemberId == customerId).FirstOrDefault();
                if (cart == null)
                {
                    throw new Exception("Not Found Cart");
                }
                else
                {
                    var product = await productRepository.Get(productId);
                    if (product != null)
                    {
                        var orderDetail = orderDetailsRepository.GetAllOrder(x => x.OrderId == cart.OrderId && x.ProductId == productId).FirstOrDefault();
                        if (orderDetail != null)
                        {
                            if (orderDetail.Quantity > 1)
                            {
                                orderDetail.Quantity -= 1;
                                orderDetail.UnitPrice -= product.UnitPrice;
                                
                                if (await orderDetailsRepository.UpdateOrder(orderDetail.OrderId, orderDetail.ProductId, orderDetail))
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new Exception("Remove Fail");
                                }
                            }
                            else
                            {
                                if (await orderDetailsRepository.DeleteOrder(orderDetail.OrderId, orderDetail.ProductId))
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new Exception("Remove Fail");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Not Found Product");
                        }
                    }
                    else
                    {
                        throw new Exception("Not Found Product");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int TotalProductOfOrder(int orderId)
        {
            try
            {
                return orderDetailsRepository.GetAllOrder(x => x.OrderId == orderId).ToList().Count();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public decimal TotalPriceOfOrder(int orderId)
        {
            try
            {
                return orderDetailsRepository.GetAllOrder(x => x.OrderId == orderId).ToList().Sum(x => x.UnitPrice);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
