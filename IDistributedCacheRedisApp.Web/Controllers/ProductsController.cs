using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product p = new Product() { Id = 2, name = "Tom2", Price = 200 };

            string jsonConvertedData = JsonConvert.SerializeObject(p);

            byte[] byteProduct = Encoding.UTF8.GetBytes(jsonConvertedData);

            _distributedCache.Set("ensttuman:3", byteProduct);

            //await _distributedCache.SetStringAsync("ensttuman:2", jsonConvertedData, options);

            

            return View();
        }

        public async Task<IActionResult> Show()
        {

            // string name = await _distributedCache.GetStringAsync("ensttuman:1");


            byte[] getByteProduct = _distributedCache.Get("ensttuman:3");

            string convertedtoString = Encoding.UTF8.GetString(getByteProduct);


            Product product = JsonConvert.DeserializeObject<Product>(convertedtoString);





            

            ViewBag.product = product;
            
            return View();
        }

        public async Task<IActionResult> Remove()
        {

            var hasCache = _distributedCache.Get("telefon");

            if (hasCache != null)
            {
                _distributedCache.Remove("telefon");
            }

            await _distributedCache.RemoveAsync("tablet");

            return View();
        }


        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/beach_300_169.jpg");

            byte[] convertedImage = System.IO.File.ReadAllBytes(path);

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            _distributedCache.Set("ImageCache", convertedImage,options);

            return View();
        }

        public IActionResult GetImage()
        {
          var byteImage =   _distributedCache.Get("ImageCache");


            return File(byteImage, "image/jpg");
        }
    }
}