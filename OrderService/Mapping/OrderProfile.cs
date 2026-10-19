using AutoMapper;
using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();

        CreateMap<CreateOrderRequest, Order>();
    }
}