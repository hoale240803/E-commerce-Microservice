using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _redisRepository;
        private readonly IDistributedCache _redisCache;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository repository, IDistributedCache redisCache, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _redisRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _redisCache = redisCache ?? throw new ArgumentException(nameof(redisCache));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentException(nameof(redisCache));
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketAsync(string userName)
        {
            var basket = await _redisRepository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasketAsync([FromBody] ShoppingCart basket)
        {
            // TODO : Communicate with Discount.Grpc
            // and Calculate latest prices of product into shopping cart
            // consume Discount Grpc

            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _redisRepository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasketAsync(string userName)
        {
            await _redisRepository.DeleteBasket(userName);

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        { 
            // get existing basket with total price
            // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket

            // get existing basket with total price
            var basket = await _redisRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            // remove the basket
            await _redisRepository.DeleteBasket(basket.UserName);

            return Accepted();
        }

        //[Obsolete]
        //[HttpGet("{userName}", Name = "GetBasket")]
        //[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        //{
        //    var basket = await _redisCache.GetStringAsync(userName);
        //    if (string.IsNullOrEmpty(basket))
        //        return null;

        //    return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        //}

        //[Obsolete]
        //[HttpPost]
        //[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        //{
        //    // TODO : Communicate with Discount.Grpc
        //    // and Calculate latest prices of product into shopping cart
        //    // consume Discount Grpc
        //    //foreach (var item in basket.Items)
        //    //{
        //    //    var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
        //    //    item.Price -= coupon.Amount;
        //    //}

        //    _redisCache.SetString(basket.UserName, JsonConvert.SerializeObject(basket));

        //    return await GetBasket(basket.UserName);
        //}

        //[Obsolete]
        //[HttpDelete("{userName}", Name = "DeleteBasket")]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> DeleteBasket(string userName)
        //{
        //    //await _redisRepository.DeleteBasket(userName);

        //    await _redisCache.RemoveAsync(userName);
        //    return Ok();
        //}
    }
}