using Microsoft.AspNetCore.Mvc;
using Redis.ExchangeApi.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.ExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(0);
        }
        public IActionResult Index()
        {            
            db.StringSet("name", "Omer Ozkan");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var redisValue = db.StringLength("name");
            //var redisValue = db.StringGetRange("name",0,2);
            //db.StringIncrement("ziyaretci", 1);

            db.StringDecrementAsync("ziyaretci", 10).Wait();
            ViewBag.Value = redisValue.ToString();

            //if (redisValue.HasValue)
            //{
            //    ViewBag.Value = redisValue.ToString();
            //}

            return View();
        }

    }
}
