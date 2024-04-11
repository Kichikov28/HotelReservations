using HotelReservations.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Services.Contracts
{
    public interface IClientsService
    {
        public Task<string> CreateClientAsync(ClientCreateViewModel model);
        public Task<ClientsIndexViewModel> GetClientsAsync(ClientsIndexViewModel model);
        public Task<ClientEditViewModel> EditCustomerByIdAsync(string id);
        public Task UpdateCustomerAsync(ClientEditViewModel model);
        public Task<ClientDetailsViewModel> GetClientDetailsByIdAsync(string id);
        public List<ClientHistoryViewModel> GetClientReservationHistory(string id);

	}
}
