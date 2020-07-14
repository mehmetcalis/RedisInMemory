using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redisServer;
        protected readonly IDatabase db;

        public BaseController(RedisService redisServer)
        {
            _redisServer = redisServer;
            db = _redisServer.GetDb(4);
        }
    }
}