using HotelReservations.Data.Models;
using HotelReservations.ViewModels.Reservations;
using HotelReservations.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Services.Contracts
{
    public interface IReservationsService
    {
        public Task CreateReservationAsync(CreateReservationViewModel model);
        public Task<IndexReservationsViewModel> GetReservationsAsync(IndexReservationsViewModel model);
        public  Task<List<SelectListRoomViewModel>> GetFreeRoomsSelectListAsync();
        public Task<List<SelectListRoomViewModel>> GetAllRoomsSelectListAsync(EditReservationViewModel model);
        public Task<int> GetRoomCapacityAsync(string id);
        public Task<Client> FindClientAsync(Client clnt);
        public Task<DetailsReservationViewModel> GetReservationDetailsAsync(string id);
    }
}
