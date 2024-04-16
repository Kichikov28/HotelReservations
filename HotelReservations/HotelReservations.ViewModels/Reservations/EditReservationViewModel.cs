using Castle.Core.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservations.Data.Models;
using HotelReservations.ViewModels.Clients;

namespace HotelReservations.ViewModels.Reservations
{
    public class EditReservationViewModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string RoomId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Accomodation date")]
        public new DateTime AccommodationDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Leave date")]
        public new DateTime LeaveDate { get; set; }
        public IList<Client> ClientsToAdd { get; set; } = new List<Client>();
        public IList<ClientIndexViewModel> ClientsToRemove { get; set; } = new List<ClientIndexViewModel>();
    }
}
