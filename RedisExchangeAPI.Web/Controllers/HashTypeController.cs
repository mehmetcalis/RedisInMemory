using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
       
        public HashTypeController(RedisService redisServer) : base(redisServer)
        {
        }

        private string ListKey = "HashListKey";

        public IActionResult Index()
        {

            Dictionary<string, string> list = new Dictionary<string, string>();
            if (db.KeyExists(ListKey))
            {
                db.HashGetAll(ListKey).ToList().ForEach(i =>
                {
                    list.Add(i.Name.ToString(), i.Value.ToString());
                });
            }

            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string key,string value)
        {
            db.HashSet(ListKey, key, value);
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string key)
        {

            if (db.KeyExists(ListKey))
            {
               await db.HashDeleteAsync(ListKey, key);
            }
            return RedirectToAction("Index");
        }
    }
}