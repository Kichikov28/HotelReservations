using HotelReservations.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Reservations
{
    public class DetailsReservationViewModel:IndexReservationViewModel
    {
        public ICollection<ClientIndexViewModel> Clients { get; set; }
    }
}
