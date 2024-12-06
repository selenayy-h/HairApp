using Microsoft.AspNetCore.Mvc;

namespace Hairr.Controllers
{
    public class WomanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
