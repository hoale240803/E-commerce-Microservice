using Microsoft.Extensions.Logging;

namespace Order.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Domain.Entities.Order> GetPreconfiguredOrders()
        {
            return new List<Domain.Entities.Order>
            {
                new Domain.Entities.Order() {
                    UserName = "swn",
                    FirstName = "Hoa",
                    LastName = "Le",
                    EmailAddress = "lehoa08121998@gmail.com",
                    AddressLine = "da nang",
                    Country = "Viet Nam",
                    TotalPrice = 350,
                    CVV="12",
                    CardName="Hoa",
                    CardNumber="424242424242",
                    CreatedBy="hoale",
                    CreatedDate=DateTime.Now,
                    PaymentMethod=1,
                    ZipCode="550000",
                    Expiration="1//1/2034",
                    LastModifiedBy="hoale",
                    LastModifiedDate=DateTime.Now,
                    State="Michigan"

                    
                }
            };
        }
    }
}