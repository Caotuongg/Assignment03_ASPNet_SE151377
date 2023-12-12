using Assignment03_BussinessObject.Entities;
using Assignment03_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<AspNetUser> GetUser(string id)
        {
            try
            {
                var check = await userRepository.Get(id);
                if (check != null)
                {
                    return check;
                }
                else
                {
                    throw new Exception("Not Found User");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<AspNetUser> GetAll()
        {
            try
            {
                return userRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Add(AspNetUser user)
        {
            try
            {
                return await userRepository.Add(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(AspNetUser user)
        {
            try
            {
                var check = await userRepository.Get(user.Id);
                if (check != null)
                {
                    check.UserName = user.UserName;
                    check.NormalizedUserName = user.NormalizedUserName;
                    check.Email = user.Email;
                    check.NormalizedEmail = user.NormalizedEmail;
                    check.PasswordHash = user.PasswordHash;
                    check.SecurityStamp = user.SecurityStamp;
                    check.ConcurrencyStamp = user.ConcurrencyStamp;
                    check.PhoneNumber = user.PhoneNumber;
                    check.FirstName = user.FirstName;
                    check.LastName = user.LastName;
                    return await userRepository.Update(user.Id, check);

                }
                else
                {
                    throw new Exception("Not Found User");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var mem = await userRepository.Get(id);
                if (mem != null)
                {
                    return await userRepository.Delete(id);

                }
                else
                {
                    throw new Exception("Not Found User");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AspNetUser> Login(string email, string password)
        {

            try
            {
                var account = userRepository.GetAll();
                return account.SingleOrDefault(x => x.Email == email && x.PasswordHash == password);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

