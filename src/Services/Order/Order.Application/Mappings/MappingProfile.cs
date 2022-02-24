using AutoMapper;
using Order.Application.Features.Orders.Commands.CheckoutOrder;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Application.Features.Orders.Queries;

namespace Order.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.Order, OrdersVm>().ReverseMap();
            CreateMap<Domain.Entities.Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Domain.Entities.Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}