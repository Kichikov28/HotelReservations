﻿using Microsoft.AspNetCore.Identity;

namespace HotelReservations.Data.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UCN { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool Status { get; set; }
        public DateTime? QuitDate { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
    