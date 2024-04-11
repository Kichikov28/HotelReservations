using HotelReservations.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Reservations
{
    public class IndexReservationsViewModel : PagingViewModel
    {
        public IndexReservationsViewModel(int elementsCount, int itemsPerPage = 5, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }

        public ICollection<IndexReservationViewModel> Reservations { get; set; }
    }
}
