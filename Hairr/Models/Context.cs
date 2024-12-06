using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Hairr.Models
{
    public class Context : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-ACDH94DG\\SQLEXPRESS01;database=HairDresser;integrated security=true;TrustServerCertificate=True;");
        }

        public DbSet<Personel> Personels { get; set; }
        public DbSet<Islem> Islems { get; set; }


        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Appointment -> Personel ilişkisinde Cascade yerine Restrict kullan
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Personel)
                .WithMany()
                .HasForeignKey(a => a.PersonelId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict

            // Appointment -> Islem ilişkisi aynı kalabilir (isteğe bağlı)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Islem)
                .WithMany()
                .HasForeignKey(a => a.IslemId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade kalabilir
        }
    }
}
