using Assignment03_BussinessObject.Entities;
using Assignment03_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Assignment03_WebClient.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration configuration;
        private string OrderUrlApi = "https://localhost:7129/api/Orders/";

        public CartController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            this.configuration = configuration;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (TempData["ErrMsg"] != null)
            {
                ViewData["ErrMsg"] = TempData["ErrMsg"].ToString();
            }
            if (!string.IsNullOrEmpty(role))
            {
                ViewData["Role"] = role;
            }
            try
            {
                var cart = new List<OrderDetail>();
                var cartString = HttpContext.Session.GetString("Cart");
                if (string.IsNullOrEmpty(cartString))
                {
                    cart = new List<OrderDetail>();
                }
                else
                {
                    cart = JsonConvert.DeserializeObject<List<OrderDetail>>(cartString);
                }
                return View(cart);
            }
            catch (Exception ex)
            {
                ViewData["ErrMsg"] = ex.Message;
                return View(new List<OrderDetail>());
            }
        }


        public async Task<IActionResult> Report()
        {
            var token1 = HttpContext.Session.GetString("Token");
            if(token1 == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                return View(new ReportVM()
                {
                    token = token1
                });
            }
        }
    }
}
