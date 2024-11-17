using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouPlay.MVC.Services.Implementations;

namespace YouPlay.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(TokenFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
