﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
    