using AutoMapper;
using Store.Domain.Entities.Orders;
using Store.Services.Abstractions.Orders;
using Store.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Mapping.Orders
{
    public class OrderProfile :Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress, OrderAddressDto>().ReverseMap();



            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));


             CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.product.PictureUrl));

            CreateMap<DeliveryMethod, DeliveryMethodResponse>();
        }
    }
}
