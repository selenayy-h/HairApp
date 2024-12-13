using Hairr.Migrations;
using Hairr.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Hairr.Controllers
{
    public class LoginController : Controller
    {
        Context c = new Context();

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(User user)
        {
            var userData = c.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);

            if (userData == null)
            {
                // Kullanıcı bulunamazsa hata göster
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(user);
            }

            // Kullanıcı doğrulandıysa kimlik oluştur
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, "User") // Rol bilgisi
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Kullanıcıyı sisteme giriş yaptır
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Giriş sonrası ana sayfaya yönlendir
            return RedirectToAction("Index", "Man");
        }


        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            // Kullanıcının çıkış yapması için mevcut oturumu sonlandırıyoruz
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Çıkış yaptıktan sonra giriş sayfasına yönlendiriyoruz
            return RedirectToAction("Index", "Login");
        }
    }
}