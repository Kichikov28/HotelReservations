using HotelReservations.Data.Models;
using HotelReservations.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Reservations
{
    public class DetailsReservationViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Room Room { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Accomodation date")]
        public DateTime AccommodationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Leave date")]
        public DateTime LeaveDate { get; set; }

        [DisplayName("Breakfast")]
        public bool HasBreakfast { get; set; }

        [DisplayName("Allinclusive")]
        public bool HasAllInclusive { get; set; }

        [Display(Name = "Total price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public ICollection<ClientIndexViewModel> Clients { get; set; }=new List<ClientIndexViewModel>();
    }
}
