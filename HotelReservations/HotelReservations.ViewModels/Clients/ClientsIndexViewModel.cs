using HotelReservations.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Clients
{
    public class ClientsIndexViewModel : PagingViewModel
    {
        public ClientsIndexViewModel(int elementsCount, int itemsPerPage = 5, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }
        public ClientsIndexViewModel() : base(0)
        {

        }
        public string FilterByName { get; set; }
        public ICollection<ClientIndexViewModel> Clients { get; set; } = new List<ClientIndexViewModel>();
    }
}
