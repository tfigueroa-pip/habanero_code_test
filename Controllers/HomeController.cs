using System.Web.Mvc;
using HabaneroCodeTest.Methods;

namespace HabaneroCodeTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? clearcache)
        {
            var getGames = new GameMethods();
            var GamesList = getGames.ListOfGames(clearcache);

            return View(GamesList);
        }

    }
}