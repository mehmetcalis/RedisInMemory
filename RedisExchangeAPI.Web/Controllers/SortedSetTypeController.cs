using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private IDatabase db;
        private string listkey="SortedCacheKey";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<String> list = new HashSet<string>();
            if (db.KeyExists(listkey))
            {
                //db.SortedSetScan(listkey).ToList().ForEach(i =>
                //{
                //    list.Add(i.ToString());
                //});
                db.SortedSetRangeByRank(listkey,order:Order.Descending).ToList().ForEach(i =>
                {
                    list.Add(i.ToString());
                });
            }



            return View(list);
        }

        public IActionResult Add(string name,int score)
        {
            db.KeyExpire(listkey, DateTime.Now.AddMinutes(10));
            db.SortedSetAdd(listkey, name, score);

            return RedirectToAction("Index");
        }


        public IActionResult DeleteItem(string name)
        {
            if (db.KeyExists(listkey))
            {
                db.SortedSetRemove(listkey, name);
            }

            return RedirectToAction("Index");
        }
    }
}