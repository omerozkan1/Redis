using Microsoft.AspNetCore.Mvc;
using Redis.ExchangeApi.Web.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace Redis.ExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if(db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x=> 
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
