using WebAppRazor.Entities;

namespace WebAppRazor.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CheckOut(Order order);

        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}