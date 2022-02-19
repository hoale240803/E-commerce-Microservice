using Microsoft.EntityFrameworkCore;
using Order.Application.Contracts.Persistence;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                                    .Where(o => o.UserName == userName)
                                    .ToListAsync();
            return orderList;
        }
    }
}