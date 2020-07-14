using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "HashCacheKey";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }

        public IActionResult Index()
        {
            HashSet<string> hashSetList = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(i =>
                {
                    hashSetList.Add(i.ToString());
                });
            }
            return View(hashSetList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            db.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string name)
        {
            if (db.KeyExists(listKey))
            {
                db.SetRemoveAsync(listKey, name).Wait();
            }
            
            return RedirectToAction("Index");
        }
    }
}