using HotelReservations.ViewModels.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Services.Contracts
{
    public interface IRoomsService
    {
        public Task<string> CreateRoomAsync(CreateRoomViewModel model);
    }
}
