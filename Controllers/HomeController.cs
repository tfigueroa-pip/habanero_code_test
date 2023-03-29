using System.Web.Mvc;
using HabaneroCodeTest.Methods;

namespace HabaneroCodeTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var getGames = new GameMethods();
            var GamesList = getGames.ListOfGames();

            return View(GamesList);
        }

    }
}