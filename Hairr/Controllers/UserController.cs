using Microsoft.AspNetCore.Mvc;

namespace Hairr.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
