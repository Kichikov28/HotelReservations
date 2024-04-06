using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Clients
{
    public class ClientCreateViewModel
    {
        [Required]
        [Display(Name = "First name")]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email")]
        [StringLength(25)]
        public string Email { get; set; }

        [Display(Name = "Adult")]
        public bool IsAdult { get; set; }
        public string ReservationId { get; set; }
    }
}
