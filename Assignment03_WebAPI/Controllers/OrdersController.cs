using Assignment03_BussinessObject.Entities;
using Assignment03_Service;
using Assignment03_WebAPI.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Assignment03_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        
        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPut]
        [Route("add-cart/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddCart(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (await orderService.AddProduct(userId, id))
                {
                    return Ok(new
                    {
                        Message = "Add Success"
                    });
                }
                else
                {
                    throw new Exception("Add Fail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("buy-cart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BuyCart()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (await orderService.BuyCart(userId))
                {
                    return Ok(new
                    {
                        Message = "Buy Success"
                    });
                }
                else
                {
                    throw new Exception("Buy Fail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get-cart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                return Ok(new
                {
                    Data = mapper.Map<OrderVM>(orderService.GetCart(userId))
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get-order-by-user/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderByUser(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                return Ok(new
                {
                    Data = mapper.Map<OrderVM>(orderService.GetOrderById(userId, id))
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get-all-order-by-user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllOrderByUser()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var orders = mapper.Map<List<OrderAllVM>>(orderService.GetOrdersOfCus(userId));
                foreach (var order in orders)
                {
                    order.TotalProduct = orderService.TotalProductOfOrder(order.OrderId.Value);
                    order.TotalPrice = orderService.TotalPriceOfOrder(order.OrderId.Value);
                }
                return Ok(new
                {
                    Data = orders
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get-order-by-admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderByAdmin(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                return Ok(new
                {
                    Data = mapper.Map<OrderVM>(orderService.GetOrderDetail(id))
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get-all-order-by-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrderByAdmin(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var orders = mapper.Map<List<OrderAllVM>>(orderService.GetOrders(startDate, endDate));
                foreach (var order in orders)
                {
                    order.TotalProduct = orderService.TotalProductOfOrder(order.OrderId.Value);
                    order.TotalPrice = orderService.TotalPriceOfOrder(order.OrderId.Value);
                }
                return Ok(new
                {
                    Data = orders
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

    }
}
