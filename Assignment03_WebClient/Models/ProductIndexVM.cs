using Assignment03_BussinessObject.Entities;

namespace Assignment03_WebClient.Models
{
    public class ProductIndexVM
    {
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int ItemPerPage { get; set; }
        public int TotalValues { get; set; }
        public string Search { get; set; }
        public IEnumerable<Product> Products { get; set; }

    }
}
