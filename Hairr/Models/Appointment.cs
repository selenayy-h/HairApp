using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hairr.Models
{
    public class Appointment
    {

        [Key]
        public int ID { get; set; } // Primary Key


        [Required]
        [ForeignKey("Islem")]
        public int IslemId { get; set; }

        public Islem? Islem { get; set; }



        [Required]
        [ForeignKey("Personel")]
        public int PersonelId { get; set; }

        public Personel? Personel { get; set; }


        public string CustomerName { get; set; } // Müşteri adı-soyadı
        public DateTime AppointmentDate { get; set; } // Randevu tarihi ve saati
        public string Status { get; set; } // Randevu durumu (Beklemede, Onaylandı, Reddedildi)

    }
}
