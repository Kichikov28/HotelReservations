using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly ApplicationDbContext context;

        public RoomsService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<string> CreateRoomAsync(CreateRoomViewModel model)
        {
            Room room = new Room()
            {
                Capacity = model.Capacity,
                IsAvailable = model.IsAvailable,
                Number = model.Number,
                PricePerAdultBed = model.PricePerAdultBed,
                PricePerChildBed = model.PricePerChildBed,
                Type = model.RoomType,
            };
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
            return room.Id;
        }
    }
}
