﻿using HotelReservations.ViewModels.Rooms;
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
        public Task<IndexRoomsViewModel> GetRoomsAsync(IndexRoomsViewModel model);
        public Task<RoomDetailsViewModel> GetRoomDetailsAsync(string id);
        public Task<EditRoomViewModel> EditRoomAsync(string id);
        public Task<string> UpdateRoomAsync(EditRoomViewModel model);
        public Task<RoomDetailsViewModel> DeleteRoomByIdAsync(string id);
        public Task DeleteConfirmRoomAsync(RoomDetailsViewModel model);
    }
}
