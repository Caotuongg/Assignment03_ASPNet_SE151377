using Assignment03_BussinessObject.Entities;
using Assignment03_Service;
using Assignment03_WebAPI.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assignment03_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct(string? search, int? pageIndex, int itemPerPage)
        {
            try
            {
                var products = await productService.GetAll(search);
                int totalPage = (int)Math.Ceiling((decimal)(products.Count() / itemPerPage));
                if (pageIndex == null || pageIndex == 0 || pageIndex > totalPage)
                {
                    pageIndex = 1;
                }
                var pro = products.Skip((pageIndex.Value - 1) * itemPerPage).Take(itemPerPage).ToList();
                return StatusCode(200, new
                {
                    Status = "Success",
                    TotalPage = totalPage,
                    PageIndex = pageIndex,
                    ItemPerPage = itemPerPage,
                    TotalValues = products.Count(),
                    Data = pro
                }); 

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var age = await productService.GetProductById(id);
                return Ok(new
                {
                    Status = "Success",
                    Data = age
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var book = mapper.Map<Product>(model);
                    var check = await productService.Add(book);
                    return check ? Ok(new
                    {
                        Status = 1,
                        Message = "Add Success!!!"
                    }) : Ok(new
                    {
                        Status = 0,
                        Message = "Add Fail!!!"
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Status = "Error",
                        Message = "Not Found",
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductUpdateVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var book = mapper.Map<Product>(model);
                    var check = await productService.Update(book);
                    return check ? Ok(new
                    {
                        Status = 1,
                        Message = "Update Success!!!"
                    }) : Ok(new
                    {
                        Status = 0,
                        Message = "Update Fail!!!"
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Status = "Error",
                        Message = "Not Found",
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var check = await productService.Delete(id);
                    return check ? Ok(new
                    {
                        Message = "Delete Success!!!"
                    }) : Ok(new
                    {
                        Message = "Delete Fail!!!"
                    });
                }
                else
                {
                    return StatusCode(404, new
                    {
                        Status = "Error",
                        Message = "Not Found",
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}

