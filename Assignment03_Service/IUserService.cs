using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public interface IUserService
    {
        Task<AspNetUser> GetUser(string id);
        IEnumerable<AspNetUser> GetAll();
        Task<bool> Add(AspNetUser user);
        Task<bool> Update(AspNetUser user);
        Task<bool> Delete(string id);
        Task<AspNetUser> Login(string email, string password);
    }
}
