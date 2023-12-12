using System;
using System.Collections.Generic;

namespace Assignment03_BussinessObject.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
