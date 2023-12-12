using Assignment03_BussinessObject.Entities;
using Assignment03_Service;
using Assignment03_WebAPI.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace Assignment03_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public UsersController(IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = userService.GetAll();
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
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(role))
                {
                    return StatusCode(401, new
                    {
                        Status = "Error",
                        Message = "You are not login",
                    });
                }
                //if (role != "Admin")
                //{
                //    return Ok(new
                //    {
                //        Status = "Success",
                //        Data = await userService.GetUser(int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value))
                //    });
                //}
                if (role == "Admin")
                {
                    return Ok(new
                    {
                        Status = "Success",
                        Data = await userService.GetUser(id)
                    });
                }

                return StatusCode(404, new
                {
                    Status = "Error",
                    Message = "Not Found",
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



        [HttpPost]
        public async Task<IActionResult> AddUser(UserVM model)
        {
            try
            {
                //var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                //if (role == "Amdin")
                //{
                var user = mapper.Map<AspNetUser>(model);
                var check = await userService.Add(user);
                return check ? Ok(new
                {
                    Status = 1,
                    Message = "Add Success!!!"
                }) : Ok(new
                {
                    Status = 0,
                    Message = "Add Fail!!!"
                });
                //}
                //else
                //{
                //    return StatusCode(404, new
                //    {
                //        Status = "Error",
                //        Message = "Not Found",
                //    });
                //}
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
        public async Task<IActionResult> UpdateUser(UserUpdateVM model)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin" || role != "Admin")
                {
                    var user = mapper.Map<AspNetUser>(model);
                    var check = await userService.Update(user);
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
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == "Admin")
                {
                    var check = await userService.Delete(id);
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

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                if (login.Email == configuration["Admin:Email"])
                {
                    if (login.Password == configuration["Admin:Password"])
                    {
                        return Ok(new
                        {
                            Status = "Login Success",
                            Role = "Admin",
                            Info = new { },
                            Token = JWTManage.GetToken("Admin", "Admin")
                        });
                    }
                }
                var mem = await userService.Login(login.Email, login.Password);
                if (mem == null)
                {
                    return StatusCode(404, new
                    {
                        Status = "Error",
                        Message = "Login Fail",
                    });
                }
                //if (mem.IsDeleted)
                //{
                //    return StatusCode(404, new
                //    {
                //        Status = "Error",
                //        Message = "Your account had been deleted in the system!!!",
                //    });
                //}
                return Ok(new
                {
                    Status = "Login Success",
                    Role = "Customer",
                    Info = mem,
                    Token = JWTManage.GetToken("Customer", mem.Id + "")
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


    }
}
