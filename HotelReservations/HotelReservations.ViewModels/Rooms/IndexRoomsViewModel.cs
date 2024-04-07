using HotelReservations.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Rooms
{
    public class IndexRoomsViewModel : PagingViewModel
    {
        public IndexRoomsViewModel(int elementsCount, int itemsPerPage = 5, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }
        public IndexRoomsViewModel():base(0)
        {
                
        }
        public ICollection<IndexRoomViewModel> Rooms { get; set; } = new List<IndexRoomViewModel>();
    }
}
