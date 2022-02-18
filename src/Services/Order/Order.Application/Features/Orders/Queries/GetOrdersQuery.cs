using MediatR;

namespace Order.Application.Features.Orders.Queries
{
    public class GetOrdersQuery : IRequest<List<OrdersVm>>
    {
        public string UserName { get; set; }

        public GetOrdersQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}