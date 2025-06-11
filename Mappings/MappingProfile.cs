using AutoMapper;
using CoreXCrud.DTOs.OrderDetailDtos;
using CoreXCrud.DTOs.OrderDtos;
using CoreXCrud.DTOs.ProductDtos;
using CoreXCrud.DTOs.UserDtos;
using CoreXCrud.Entities;

namespace CoreXCrud.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
        }
    }
}
