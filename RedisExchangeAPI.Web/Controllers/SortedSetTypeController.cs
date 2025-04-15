using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "names";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                //db.SortedSetScan(listKey).ToList().ForEach (x =>
                //{
                //    namesList.Add(x.Element.ToString());
                //});
                //

                db.SortedSetRangeByRank(listKey,order:Order.Descending).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(string name, double score)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            db.SortedSetAdd(listKey, name, score);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemove(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
