﻿using AutoMapper;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO;
using Warehouse_Management.Models.DTO.Lot;
using Warehouse_Management.Models.DTO.Order;
using Warehouse_Management.Models.DTO.OrderItem;
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
            CreateMap<CreateShelfDTO, Shelf>();

            // **Lot Mapping**
            CreateMap<CreateLotDTO, Lot>();

            CreateMap<Lot, LotDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.ProductName))
                .ForMember(dest => dest.ShelfName, opt => opt.MapFrom(src => src.Shelf!.Name));

            CreateMap<UpdateLotDTO, Lot>();
        }

    }
}
