﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {

        private readonly RedisService _redisService;

        private readonly IDatabase db;
        private string listKey = "hashnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }

        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
                return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        { //her seferinde süreç 0'lanması için KeyExist kullanılır
          //if (!db.KeyExists(listKey))
          //{
          //    db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
          //}
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            db.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteItem(string name)
        {
            db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
