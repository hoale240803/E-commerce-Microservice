
namespace Order.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Domain.Entities.Order>
    {
        Task<IEnumerable<Domain.Entities.Order>> GetOrdersByUserName(string userName);
    }
} 