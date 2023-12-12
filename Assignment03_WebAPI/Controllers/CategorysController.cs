using Assignment03_Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment03_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        
        public CategorysController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = categoryService.GetAll();
                //var author = new List<Author>();
                //foreach (var item in list)
                //{
                //    author.Add(mapper.Map<Author>(item));
                //}
                return Ok(new
                {
                    Status = "Success",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = ex.Message,
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var age = await categoryService.GetCategory(id);
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
    }
}
