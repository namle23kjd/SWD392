using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO;
using Warehouse_Management.Models.DTO.Lot;
using Warehouse_Management.Models.DTO.Order;
using Warehouse_Management.Models.DTO.OrderItem;
using Warehouse_Management.Models.DTO.Platform;
using Warehouse_Management.Models.DTO.PlatformDTO;
using Warehouse_Management.Models.DTO.Product;
using Warehouse_Management.Models.DTO.Shelf;
using Warehouse_Management.Models.DTO.StockTransaction;
using Warehouse_Management.Models.DTO.Supplier;

namespace Warehouse_Management.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityUser, UserDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.LockoutEnd == null || src.LockoutEnd <= DateTimeOffset.UtcNow))
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<RegisterRequestDTO, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));

            CreateMap<Product, ProductDTO>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Unknown"))
                 .ReverseMap();
            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<Product, UpdateProductDTO>().ReverseMap();

            CreateMap<CreateOrderDTO, Order>()
           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<CreateOrderItemDTO, OrderItem>();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice)); // ✅ Map UnitPrice từ OrderItem


            CreateMap<UpdateOrderDTO, Order>()
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
           .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

           
            CreateMap<UpdateOrderItemDTO, OrderItem>();


            CreateMap<StockTransaction, StockTransactionDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.ProductName))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier!.Name))
            .ForMember(dest => dest.LotCode, opt => opt.MapFrom(src => src.Lot!.LotCode))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName));

            CreateMap<CreateStockTransactionDTO, StockTransaction>();

            CreateMap<Supplier, SupplierDTO>();
            CreateMap<CreateSupplierDTO, Supplier>();
            CreateMap<UpdateSupplierDTO, Supplier>();
           

            CreateMap<Shelf, ShelfDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName));
            CreateMap<CreateShelfDTO, Shelf>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());


            CreateMap<CreateLotDTO, Lot>()
               .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Lot, LotDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.ProductName))
                .ForMember(dest => dest.ShelfName, opt => opt.MapFrom(src => src.Shelf!.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Unknown"));

            CreateMap<UpdateLotDTO, Lot>();

            CreateMap<Platform, PlatfromDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => src.ApiKey))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<CreatePlatformDTO, Platform>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => src.ApiKey))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<UpdatePlatformDTO, Platform>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => src.ApiKey))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }

    }
}
