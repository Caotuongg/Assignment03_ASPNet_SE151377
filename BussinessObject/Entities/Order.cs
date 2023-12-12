using System;
using System.Collections.Generic;

namespace Assignment03_BussinessObject.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string? MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public bool ? IsPayment { get; set; }
        public bool ? IsDeleted { get; set; }
        public virtual AspNetUser? Member { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
