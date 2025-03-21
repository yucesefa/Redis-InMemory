using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration=DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 1, Name = "Product 1", Price = 100 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:1", byteProduct, cacheOptions);

            //_distributedCache.SetString("product:1", jsonProduct, cacheOptions);


            //await _distributedCache.SetStringAsync("name", "Product 2",cacheOptions);
            return View();
        }
        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");
            //ViewBag.name = name;

            Byte[] byteProduct = _distributedCache.Get("product:1");

            //string jsonProduct = _distributedCache.GetString("product:1");

            string jsonProduct = Encoding.UTF8.GetString(byteProduct);
            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = product;

            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/1.jpeg");
            
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image:1", imageByte);

            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("image:1");
            return File(imageByte, "image/jpg");
        }
    }
}
