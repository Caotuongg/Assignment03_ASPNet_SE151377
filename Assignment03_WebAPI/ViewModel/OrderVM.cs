namespace Assignment03_WebAPI.ViewModel
{
    
        public class OrderAllVM
        {
            public int? OrderId { get; set; }
            public string? UserId { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? RequiredDate { get; set; }
            public DateTime? ShippedDate { get; set; }
            public int? TotalProduct { get; set; }
            public decimal? TotalPrice { get; set; }
            public bool? IsDeleted { get; set; }
            public bool? IsPayment { get; set; }
        }
        public class OrderVM
        {
            public int? OrderId { get; set; }
            public string? UserId { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? RequiredDate { get; set; }
            public DateTime? ShippedDate { get; set; }
            public bool? IsDeleted { get; set; }
            public bool? IsPayment { get; set; }
            public virtual ICollection<OrderDetailVM>? OrderDetails { get; set; }
        }
        public class OrderDetailVM
        {
            public int? OrderId { get; set; }
            public int? ProductId { get; set; }
            public decimal? UnitPrice { get; set; }
            public int? Quantity { get; set; }
            public float? Discount { get; set; }
            public bool? IsDeleted { get; set; }
            public virtual ProductVM? Product { get; set; }
        }
    
}
