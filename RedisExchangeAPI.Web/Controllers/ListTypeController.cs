﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        { 
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            List<string> nameList = new List<string>();

            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x => nameList.Add(x.ToString()));
            };
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteItem(string name)
        {
            db.ListRemove(listKey, name);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey);
            return RedirectToAction("Index");
        }
    }
}
