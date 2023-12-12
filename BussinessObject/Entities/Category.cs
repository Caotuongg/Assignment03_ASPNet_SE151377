using System;
using System.Collections.Generic;

namespace Assignment03_BussinessObject.Entities
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
