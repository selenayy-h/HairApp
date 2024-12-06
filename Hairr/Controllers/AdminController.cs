using Hairr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Hairr.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin rolündeki kullanıcılar erişebilir
    public class AdminController : Controller
    {
        private readonly Context c = new Context();

        // Bekleyen Randevular Sayfası
        public IActionResult Index()
        {
            // Bekleme durumundaki randevuları listele
            var bekleyenRandevular = c.Appointments.Where(x => x.Status == "Beklemede").ToList();
            return View(bekleyenRandevular);
        }

        // Randevu Detay ve Onaylama Sayfası
        public IActionResult Onayla(int id)
        {
            // İlgili randevuyu getir
            var randevu = c.Appointments.FirstOrDefault(x => x.ID == id);

            if (randevu == null)
            {
                return NotFound(); // Eğer randevu bulunamazsa 404 döner
            }

            return View(randevu); // Randevuyu Onayla sayfasına gönder
        }

        [HttpPost]
        public IActionResult Onayla(int id, bool onayDurumu)
        {
            // İlgili randevuyu getir
            var randevu = c.Appointments.FirstOrDefault(x => x.ID == id);

            if (randevu != null)
            {
                // Onay Durumuna göre işlem yap
                randevu.Status = onayDurumu ? "Onaylandı" : "Reddedildi";
                c.SaveChanges();

                TempData["Mesaj"] = $"Randevu başarıyla {(onayDurumu ? "onaylandı" : "reddedildi")}.";
            }
            else
            {
                TempData["Hata"] = "Randevu bulunamadı!";
            }

            return RedirectToAction("Index"); // İşlem sonrası listeye geri dön
        }
    }
}
