﻿using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _redisRepository;
        private readonly IDistributedCache _redisCache;

        public BasketController(IBasketRepository repository, IDistributedCache redisCache)
        {
            _redisRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _redisCache = redisCache ?? throw new ArgumentException(nameof(redisCache));
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




        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketAsync(string userName)
        {
            var basket = await _redisRepository.GetBasket(userName);

            return Ok(basket?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasketAsync([FromBody] ShoppingCart basket)
        {
            // TODO : Communicate with Discount.Grpc
            // and Calculate latest prices of product into shopping cart
            // consume Discount Grpc

            //foreach (var item in basket.Items)
            //{
            //    var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            //    item.Price -= coupon.Amount;
            //}

            return Ok(await _redisRepository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasketAsync(string userName)
        {
            await _redisRepository.DeleteBasket(userName);

            return Ok();
        }
    }
}