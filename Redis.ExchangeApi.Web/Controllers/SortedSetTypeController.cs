using Microsoft.AspNetCore.Mvc;
using Redis.ExchangeApi.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redis.ExchangeApi.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private string listKey = "sortedSetNames";
        public SortedSetTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                //db.SortedSetScan(listKey).ToList().ForEach(x=> 
                //{
                //    list.Add(x.ToString());
                //});
                db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            db.SortedSetAdd(listKey, name, score);
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemove(listKey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
