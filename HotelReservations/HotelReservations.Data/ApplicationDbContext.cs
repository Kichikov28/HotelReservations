using HotelReservations.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace HotelReservations.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext()
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithOne(r => r.Reservation)
                .HasForeignKey<Room>(r => r.ReservationId);

            builder.Entity<Client>()
                .HasOne(c => c.Reservation)
                .WithMany(r => r.Clients)
                .HasForeignKey(c => c.ReservationId);
            base.OnModelCreating(builder);  
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
            }
        }
    }

}
