using AutoMapper;
using EventBus.Messages.Events;
using Order.Application.Features.Orders.Commands.CheckoutOrder;

namespace Order.API.Mapping
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}