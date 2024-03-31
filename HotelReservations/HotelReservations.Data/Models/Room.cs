using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Data.Models
{
    public class Room
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Capacity { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public double PricePerAdultBed { get; set; }
        public double PricePerChildBed { get; set; }
        public int Number { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
