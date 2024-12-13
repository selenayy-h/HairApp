using Hairr.Models;
using Microsoft.AspNetCore.Mvc;

public class RegisterController : Controller
{
    private readonly Context _context = new Context(); // Manuel olarak oluşturma

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(User user)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Login");
        }

        return View(user);
    }
}
