using Hairr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hairr.Controllers
{
    public class ManController : Controller
    {

        Context c = new Context();
        [Authorize(Roles = "K")] // Yalnızca adminler erişebilir
        public IActionResult Index()
        {
            var degerler = c.Islems.ToList();

            return View(degerler);
        }


        public IActionResult PersonelDetay(int id)
        {
            var degerler = c.Personels.Where(x => x.IslemId == id).ToList();


            var brmad = c.Islems.Where(x => x.ID == id).Select(y => y.IslemAdi).FirstOrDefault();

            ViewBag.brm = brmad;
            return View(degerler);
        }



        public IActionResult RandevuOlustur(int id)
        {
            // İşlem detaylarını getir
            var islem = c.Islems.FirstOrDefault(x => x.ID == id);

            // İşlemi yapan personelleri getir
            var personeller = c.Personels.Where(x => x.IslemId == id).ToList();

            // Başlangıç tarihini oluştur (şu anki tarih ve saat)
            ViewData["StartDate"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

            // ViewBag ile işlem adı ve personel bilgilerini gönderiyoruz
            ViewData["Islem"] = islem;
            ViewData["Personellers"] = personeller;

            return View();
        }



        [HttpPost]
        public IActionResult RandevuOlustur(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Randevunun varsayılan durumu "Beklemede" olacak
                appointment.Status = "Beklemede";

                // Randevuyu veritabanına ekle
                c.Appointments.Add(appointment);
                c.SaveChanges();

                // Kullanıcıya bilgi mesajı göstermek için TempData kullanıyoruz
                TempData["SuccessMessage"] = "Randevu talebiniz alınmıştır. Onay bekleniyor.";

                // Başarıyla tamamlandıktan sonra Index sayfasına yönlendirelim
                return RedirectToAction("Index");
            }

            // Hata durumunda formu yeniden göster
            var islem = c.Islems.FirstOrDefault(x => x.ID == appointment.IslemId);
            var personeller = c.Personels.Where(x => x.IslemId == appointment.IslemId).ToList();
            ViewBag.Islem = islem;
            ViewBag.Personeller = personeller;

            return View(appointment);
        }


    }
}
