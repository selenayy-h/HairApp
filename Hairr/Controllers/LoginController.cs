using Hairr.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> Index(Admin p)
		{
			// Veritabanında kullanıcı adı ve şifreye göre Admin kaydını kontrol ediyoruz
			var datavalue = c.Admins.FirstOrDefault(x => x.UserName == p.UserName && x.Password == p.Password);

			if (datavalue == null)
			{
				// Kullanıcı adı veya şifre hatalıysa Error View'ını döndürüyoruz
				return View("Error");
			}

			// Kullanıcı kimlik bilgilerini oluşturuyoruz
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, p.UserName),

                 new Claim(ClaimTypes.Role, datavalue.Role) // Rol bilgisi SONRADANNN EKLENDİ
			};

			// Kullanıcı kimlik bilgileriyle bir ClaimsIdentity oluşturuyoruz
			var userIdentity = new ClaimsIdentity(claims, "Login");

			// Kullanıcıyı temsil eden bir ClaimsPrincipal oluşturuyoruz
			ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

			// Kullanıcıyı sisteme giriş yaptırıyoruz
			await HttpContext.SignInAsync(principal);

			// Başarılı giriş sonrası başka bir sayfaya yönlendirme yapıyoruz
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
