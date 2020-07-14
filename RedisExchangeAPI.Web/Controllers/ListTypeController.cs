using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string Listkey="CacheKey_Names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }

        public IActionResult Index()
        {
            List<string> nameList = new List<string>();

            if (db.KeyExists(Listkey))
            {
                db.ListRange(Listkey).ToList().ForEach(i =>
                {
                    nameList.Add(i.ToString());
                });
            }
           

            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListLeftPush(Listkey, name);

            return RedirectToAction("Index");
        }
        
        public IActionResult Remove(string name)
        {

            db.ListRemoveAsync(Listkey, name).Wait();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItemFirstOrLast(string DeletePart)
        {
            switch (DeletePart)
            {
                case "First":
                    db.ListLeftPop(Listkey);
                    break;
                case "Last":
                    db.ListRightPop(Listkey);
                    break;
                default:
                    break;
            }


            return RedirectToAction("Index");
        }
    }
}