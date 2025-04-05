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
            var db = _redisService.GetDb(0);
            db.StringSet("name", "Safa Yüce");

            return View();
        }
        public IActionResult Show()
        {
            var value = db.StringGet("name");

            Byte[] imgType = default(byte[]);
            db.StringSet("img", imgType);

            // db.StringIncrement("visitor", 100);
            // var count=db.StringIncrementAsync("visitor", 1).Result;
            db.StringDecrementAsync("visitor", 1).Wait();

            db.StringIncrement("visitor",1);
            if (value.HasValue)
            {
                ViewBag.Name = value.ToString();
            }
            else
            {
                ViewBag.Name = "Key not found";
            }
            return View();
        }
    }
}
