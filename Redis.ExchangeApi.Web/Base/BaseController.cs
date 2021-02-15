using Microsoft.AspNetCore.Mvc;
using Redis.ExchangeApi.Web.Services;
using StackExchange.Redis;

namespace Redis.ExchangeApi.Web.Base
{
    public class BaseController : Controller
    {
        private readonly RedisService redisService;
        protected readonly IDatabase db;
        public BaseController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(1);
        }
    }
}
