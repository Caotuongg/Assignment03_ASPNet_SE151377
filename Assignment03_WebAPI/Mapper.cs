using Assignment03_BussinessObject.Entities;
using Assignment03_WebAPI.ViewModel;
using AutoMapper;

namespace Assignment03_WebAPI
{
    public class Mapper : Profile
    {
        public Mapper() 
        {
            CreateMap<Product, ProductVM>();
            CreateMap<ProductVM, Product>();
            CreateMap<Product, ProductUpdateVM>();
            CreateMap<ProductUpdateVM, Product>();

            CreateMap<AspNetUser, UserVM>().ReverseMap();
            CreateMap<AspNetUser, UserUpdateVM>().ReverseMap();

            CreateMap<Order, OrderAllVM>();
            CreateMap<OrderAllVM, Order>();

            CreateMap<Order, OrderVM>();
            CreateMap<OrderVM, Order>();

            CreateMap<OrderDetail, OrderDetailVM>();
            CreateMap<OrderDetailVM, OrderDetail>();
        }
        
    }
}
