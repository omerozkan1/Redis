using Microsoft.AspNetCore.Mvc;
using Redis.ExchangeApi.Web.Base;
using Redis.ExchangeApi.Web.Services;
using System.Collections.Generic;
using System.Linq;

namespace Redis.ExchangeApi.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashKey { get; set; } = "sozluk";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (db.KeyExists(hashKey))
            {
                db.HashGetAll(hashKey).ToList().ForEach(x => 
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View(list);
        }

        public IActionResult Add(string name, string val)
        {
            db.HashSet(hashKey, name, val);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteItem(string name)
        {
            db.HashDelete(hashKey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
