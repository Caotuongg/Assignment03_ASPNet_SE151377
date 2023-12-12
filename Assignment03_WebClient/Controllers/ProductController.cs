using Assignment03_BussinessObject.Entities;
using Assignment03_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text.Json;

namespace Assignment03_WebClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private string ProductApiUrl = "https://localhost:7129/api/Products/";
        private string CategoryApiUrl = "https://localhost:7129/api/Categorys/";
        public ProductController()
        {
            _httpClient = new HttpClient();
        }

        private async Task<List<Category>> GetCategorys()
        {
            HttpResponseMessage message = await _httpClient.GetAsync(CategoryApiUrl + "GetAll");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string resData = await message.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                var categories = JsonConvert.DeserializeObject<List<Category>>(json["data"].ToString());

                return categories;
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? productId)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(ProductApiUrl + $"GetProductById?id={productId}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertProduct(resData);
                ViewData["PublisherId"] = new SelectList(await GetCategorys(), "CategoryId", "CategoryName");
                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            ViewData["PublisherId"] = new SelectList(await GetCategorys(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product book)
        {
            try
            {
                CheckAdmin();

                var role = HttpContext.Session.GetString("Role");
                ViewData["Role"] = role;

                //CheckValidate(book);

                var token = HttpContext.Session.GetString("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage message = await _httpClient.PostAsJsonAsync(ProductApiUrl + "AddProduct", book);

                if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "User");
                }
                if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    throw new Exception(json["message"].ToString());

                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewData["ErrMsg"] = ex.Message;
                ViewData["PublisherId"] = new SelectList(await GetCategorys(), "CategoryId", "CategoryName", book.CategoryId);
                return View(book);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(ProductApiUrl + $"GetProductById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertProduct(resData);

                ViewData["PublisherId"] = new SelectList(await GetCategorys(), "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound || message.StatusCode == System.Net.HttpStatusCode.InternalServerError || message.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product book)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            try
            {

                //CheckValidate(book);

                var token = HttpContext.Session.GetString("Token");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage message = await _httpClient.PutAsJsonAsync(ProductApiUrl + $"UpdateProduct?id={book.ProductId}", book);


                if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "Users");
                }

                if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    throw new Exception(json["message"].ToString());
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["PublisherId"] = new SelectList(await GetCategorys(), "CategoryId", "CategoryName", book.CategoryId);
                ViewData["ErrMsg"] = ex.Message;
                return View(book);
            }
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            CheckAdmin();

            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            HttpResponseMessage message = await _httpClient.GetAsync(ProductApiUrl + $"GetProductById?id={id}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resData = await message.Content.ReadAsStringAsync();

                var product = ConvertProduct(resData);
                ViewData["PublisherId"] = new SelectList(await GetCategorys(), "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
            if (message.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int productId)
        {
            CheckAdmin();

            var token = HttpContext.Session.GetString("Token");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await _httpClient.DeleteAsync(ProductApiUrl + $"DeleteProduct?id={productId}");

            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }

            if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "User");
            }

            if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                HttpResponseMessage getProduct = await _httpClient.GetAsync(ProductApiUrl + $"GetProductById?id={productId}");

                if (message.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resDataPro = await getProduct.Content.ReadAsStringAsync();

                    var product = ConvertProduct(resDataPro);

                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    ViewData["ErrMsg"] = json["message"].ToString();

                    return View(product);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private Product ConvertProduct(string resData)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            var product = JsonConvert.DeserializeObject<Product>(json["data"].ToString());

            return product;
        }


        private void CheckAdmin()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                RedirectToAction("Login", "Users");
            }
            if (role != "Admin")
            {
                RedirectToAction("UserIndex", "Home");
            }
        }
        [HttpGet]
        public IActionResult Error()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            return View();
        }
        public async Task<IActionResult> Index(string search, int pageIndex, int itemPerPage)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            HttpResponseMessage message;
            if (itemPerPage == null || itemPerPage == 0)
            {
                itemPerPage = 5;
            }
            if (string.IsNullOrEmpty(search))
            {
                message = await _httpClient.GetAsync(ProductApiUrl + $"GetAllProduct?pageIndex={pageIndex}&itemPerPage={itemPerPage}");
            }
            else
            {
                message = await _httpClient.GetAsync(ProductApiUrl + $"GetAllProduct?search={search}&pageIndex={pageIndex}&itemPerPage={itemPerPage}");
            }

            string resData = await message.Content.ReadAsStringAsync();

            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            if (message.StatusCode == System.Net.HttpStatusCode.InternalServerError || message.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception("Error");
            }


            int totalPage = 0;


            totalPage = int.Parse(json["totalPage"].ToString());

            pageIndex = int.Parse(json["pageIndex"].ToString());

            int totalValues = int.Parse(json["totalValues"].ToString());

            itemPerPage = int.Parse(json["itemPerPage"].ToString());

            var pros = ConvertProducts(resData);
            return View(new ProductIndexVM
            {
                PageIndex = pageIndex,
                Products = pros,
                ItemPerPage = itemPerPage,
                TotalValues = totalValues,
                Search = search,
                TotalPage = totalPage
            });
        }

        private List<Product> ConvertProducts(string resData)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            var products = JsonConvert.DeserializeObject<List<Product>>(json["data"].ToString());

            return products;
        }
    }
}
