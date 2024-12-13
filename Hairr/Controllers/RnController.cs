using Hairr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hairr.Controllers
{
    public class RnController : Controller
    {
        // Context'i doğrudan new kullanarak tanımladık
        private Context c = new Context();

        // Index Görünümü
        [Authorize(Roles = "K")] // Yalnızca adminler erişebilir
        public IActionResult Index()
        {
            var randevular = c.Appointments
                .Include(a => a.Islem)
                .Include(a => a.Personel)
                .ToList();
            return View(randevular);
        }

        [HttpGet]
        public IActionResult YeniRandevu()
        {
            ViewBag.IslemListesi = c.Islems.Select(x => new SelectListItem
            {
                Text = x.IslemAdi,
                Value = x.ID.ToString()
            }).ToList();

            ViewBag.PersonelListesi = c.Personels.Select(x => new SelectListItem
            {
                Text = $"{x.Ad} {x.Soyad}",
                Value = x.PersonelId.ToString()
            }).ToList();

            return View();
        }



        [HttpPost]
        public IActionResult YeniRandevu(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Personel = c.Personels.Find(appointment.PersonelId);
                appointment.Islem = c.Islems.Find(appointment.ID);
                c.Appointments.Add(appointment);
                c.SaveChanges();

                TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                // Hata varsa dropdown'ları yeniden doldur
                ViewBag.IslemListesi = c.Islems.Select(x => new SelectListItem
                {
                    Text = x.IslemAdi,
                    Value = x.ID.ToString()
                }).ToList();

                ViewBag.PersonelListesi = c.Personels.Select(x => new SelectListItem
                {
                    Text = $"{x.Ad} {x.Soyad}",
                    Value = x.PersonelId.ToString()
                }).ToList();

                return View(appointment);
            }
        }


        // Personel Detay Görünümü
        public IActionResult PersonelDetay(int id)
        {
            var detayPersonel = c.Personels.Where(x => x.IslemId == id).ToList();
            var brmad = c.Islems.Where(x => x.ID == id).Select(y => y.IslemAdi).FirstOrDefault();

            ViewBag.brm = brmad;
            return View(detayPersonel);
        }


        public IActionResult RandevuListesi()
        {
            IList<Appointment> randevular = c.Appointments.ToList();
            return View(randevular);
        }

    }
}
