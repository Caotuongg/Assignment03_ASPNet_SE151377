using Assignment03_BussinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_Repositories.IRepositories
{
    public interface IUserRepository: IGenericRepository<AspNetUser>
    {
    }
}
