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
            //zaman keyi var mı 1.yol
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }
            //2.yol
            if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.Remove("zaman");//bu keye sahip datayı memomry den siler

            //bu keye sahip değeri almaya çalışır eğer yoksa oluşturur
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                entry.AbsoluteExpiration = DateTime.Now.AddSeconds(10); // ekstra işlemler yapmak için var entry
                return DateTime.Now.ToString();
            });


            ViewBag.zaman=_memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
