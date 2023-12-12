using Assignment03_BussinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Assignment03_WebClient.Controllers
{
    public class UserController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration configuration;
        private string UserApiUrl = "https://localhost:7129/api/Users/";

        public UserController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            this.configuration = configuration;
        }

        public IActionResult Login()
        {
            try
            {
                if (!string.IsNullOrEmpty(TempData["LoginFail"] as string))
                {
                    throw new Exception(TempData["LoginFail"] as string);
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewData["ErrMsg"] = ex.Message;
                return View();
            }
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            HttpResponseMessage message = await _httpClient.PostAsJsonAsync(UserApiUrl + "Login", new
            {
                email = email,
                password = password
            });



            string resData = await message.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

            if (message.StatusCode == HttpStatusCode.NotFound || message.StatusCode == HttpStatusCode.InternalServerError)
            {
                TempData["LoginFail"] = json["message"].ToString();
                return RedirectToAction(nameof(Login));
            }
            if (message.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(json["token"].ToString());
                HttpContext.Session.SetString("Token", tokenJson["token"].ToString());
                HttpContext.Session.SetString("RefreshToken", tokenJson["refreshToken"].ToString());

                HttpContext.Session.SetString("Role", json["role"].ToString());

                if (json["role"].ToString() == "Admin")
                {
                    HttpContext.Session.SetString("Name", "Admin");
                    return RedirectToAction("AdminIndex", "Home");
                }
                else if (json["role"].ToString() == "Customer")
                {
                    HttpContext.Session.SetString("Name", "Customer");
                    return RedirectToAction("UserIndex", "Home");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "User");
            }
            ViewData["Role"] = role;
            string url = "";
            if (role != "Admin")
            {
                url = UserApiUrl + "GetUserById";
            }
            else
            {
                url = UserApiUrl + $"GetUserById?id={id}";
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

            HttpResponseMessage message = await _httpClient.GetAsync(url);

            if (message.StatusCode == HttpStatusCode.OK)
            {
                string resData = await message.Content.ReadAsStringAsync();

                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                var mem = JsonConvert.DeserializeObject<AspNetUser>(json["data"].ToString());

                return View(mem);
            }
            else if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "User");
            }
            return RedirectToAction("AdminIndex", "Home");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "User");
            }
            ViewData["Role"] = role;
            if (role == "Admin")
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                HttpResponseMessage message = await _httpClient.GetAsync(UserApiUrl + $"GetUserById?id={id}");

                if (message.StatusCode == HttpStatusCode.OK)
                {
                    string resData = await message.Content.ReadAsStringAsync();

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);

                    var mem = JsonConvert.DeserializeObject<AspNetUser>(json["data"].ToString());

                    return View(mem);
                }
                else if (message.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "User");
                }
                else if (message.StatusCode == HttpStatusCode.InternalServerError || message.StatusCode == HttpStatusCode.NotFound)
                {
                    string resData = await message.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(resData);
                    ViewData["ErrMsg"] = json["message"];

                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("UserIndex", "Home");
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role))
            {
                TempData["LoginFail"] = "You are not login";
                return RedirectToAction("Login", "User");
            }
            if (role == "Admin")
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                HttpResponseMessage message = await _httpClient.DeleteAsync(UserApiUrl + $"DeleteUser?id={id}");

                if (message.StatusCode == HttpStatusCode.OK)
                {

                    return RedirectToAction(nameof(Index));
                }
                else if (message.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["LoginFail"] = "You are not login";
                    return RedirectToAction("Login", "User");
                }
            }
            return RedirectToAction("UserIndex", "Home");
        }
    }
}
