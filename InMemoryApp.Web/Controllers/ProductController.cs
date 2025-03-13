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
            ////zaman keyi var mı 1.yol
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            //2.yol
            //if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            //{
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10); //zaman isminde key oluştuğu zaman burada belirtilen süre kadar cache de kalacak

            //options.SlidingExpiration = TimeSpan.FromSeconds(10); //zaman isminde key oluştuğu zaman burada belirtilen süre kadar cache de kullanılmadığı sürece cache de kalacak

            options.Priority = CacheItemPriority.High; //cache de yer kalmadığında bu keyi silme önceliği high silme low ilk bunu sil neverremove asla silme normal ise high ile low arasında önceliği var

            options.RegisterPostEvictionCallback((key, value, reason, state) => //cache den silindiğinde (silinme sebebi) çalışacak metot
            {
                _memoryCache.Set("callback", $"{key}=>{value}=>sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);
            //}

            Product product = new Product
            {
                Id = 1,
                Name = "Kalem",
                Price = 100
            };
            _memoryCache.Set<Product>("product:1", product);


            return View();
        }
        public IActionResult Show()
        {
            //_memoryCache.Remove("zaman");//bu keye sahip datayı memomry den siler

            ////bu keye sahip değeri almaya çalışır eğer yoksa oluşturur
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    entry.AbsoluteExpiration = DateTime.Now.AddSeconds(10); // ekstra işlemler yapmak için var entry
            //    return DateTime.Now.ToString();
            //});

            //ViewBag.zaman=_memoryCache.Get<string>("zaman");
            
            _memoryCache.TryGetValue("zaman", out string zamancache);

            _memoryCache.TryGetValue("callback", out string callbackcache);

            ViewBag.callback = callbackcache;

            ViewBag.zaman = zamancache;

            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();
        }
    }
}
