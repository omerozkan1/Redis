using Microsoft.AspNetCore.Mvc;
using Redis.ExchangeApi.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.ExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private string listKey = "setNames";
        public SetTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            db = redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));         
            db.SetAdd(listKey, name);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.SetRemoveAsync(listKey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
