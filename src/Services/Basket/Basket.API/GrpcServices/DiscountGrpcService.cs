using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {

        private DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        public async Task<CouponModel> GetDiscount(string productName) { 
        
            var discountReq= new GetDiscountRequest { ProductName = productName };

            return await _discountProtoService.GetDiscountAsync(discountReq);
        }

    }
}
