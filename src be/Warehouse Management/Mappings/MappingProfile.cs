using AutoMapper;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO;
using Warehouse_Management.Models.DTO.Order;
using Warehouse_Management.Models.DTO.OrderItem;
using Warehouse_Management.Models.DTO.Product;

namespace Warehouse_Management.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestDTO, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();

            CreateMap<CreateOrderDTO, Order>()
           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<CreateOrderItemDTO, OrderItem>();

            CreateMap<OrderItem, OrderItemDTO>();

            CreateMap<UpdateOrderDTO, Order>()
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  // Không thay đổi CreatedAt khi cập nhật
           .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            // Ánh xạ từ UpdateOrderItemDTO sang OrderItem (cập nhật các mục trong đơn hàng)
            CreateMap<UpdateOrderItemDTO, OrderItem>();
        }

    }
}
