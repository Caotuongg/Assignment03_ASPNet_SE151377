using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment03_BussinessObject.Entities
{
    public class UserModel : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
