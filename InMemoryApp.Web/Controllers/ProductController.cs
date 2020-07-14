using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            options.AbsoluteExpiration = DateTime.Now.AddSeconds(5);

            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set<string>("callback", $"{key}-{value} => Sebeb:{reason}- state: {state}");
            });

            Product product = new Product() { ID = 1, Name = "Product1", Price = 500 };

            _memoryCache.Set<Product>("CurrentProduct", product);

            _memoryCache.Set<String>("zaman", DateTime.Now.ToString(),options); 
            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);
            
            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;

            _memoryCache.TryGetValue<Product>("CurrentProduct", out Product product);

            ViewBag.currentproduct = product;




            return View();
        }
    }
}