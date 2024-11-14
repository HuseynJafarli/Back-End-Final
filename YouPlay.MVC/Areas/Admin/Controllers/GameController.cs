using Microsoft.AspNetCore.Mvc;

namespace YouPlay.MVC.Areas.Admin.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
