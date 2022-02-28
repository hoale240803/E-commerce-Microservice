using Microsoft.EntityFrameworkCore;
using WebAppRazor.Data;
using WebAppRazor.Entities;

namespace WebAppRazor.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly WebAppRazorContext _dbContext;

        public OrderRepository(WebAppRazorContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Order> CheckOut(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                            .Where(o => o.UserName == userName)
                            .ToListAsync();

            return orderList;
        }
    }
}