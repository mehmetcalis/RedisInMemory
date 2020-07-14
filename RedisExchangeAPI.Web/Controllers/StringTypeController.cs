using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
           
            db.StringSet("name", "mehmet çalış");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {

            var value = db.StringGet("name");
            var coutvisiter = db.StringIncrement("ziyaretci", 1);
            var stringPart = db.StringGetRange("name", 2, 4);
           

            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
                ViewBag.count = coutvisiter.ToString();
                ViewBag.stringPart = stringPart.ToString();
            }

            return View();
        }
    }
}