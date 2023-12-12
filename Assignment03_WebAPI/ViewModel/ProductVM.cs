using Assignment03_BussinessObject.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment03_WebAPI.ViewModel
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

       
    }
    public class ProductUpdateVM
    {
        public int ProductId { get; set; }
        //public int CategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

       
    }
}
