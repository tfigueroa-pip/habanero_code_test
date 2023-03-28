using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HabaneroCodeTest.Methods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HabaneroCodeTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var getGames = new GameMethods();
            var GamesList = getGames.ListOfGames();

            //var TextMessage = JsonConvert.SerializeObject(GamesList);
            //ViewBag.TextMessage = TextMessage;
            return View(GamesList);
        }

    }
}