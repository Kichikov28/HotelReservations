using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Data.Models.Enums;
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
        //public async Task<IndexRoomsViewModel> GetRoomsAsync(IndexRoomsViewModel model)
        //{
        //    model.Rooms = await this.context.Rooms
        //        .Where(x => x.Capacity >= model.Filter.Capacity)
        //        .Where(x => model.Filter.Type != null ? x.RoomType == Enum.Parse<RoomType>(model.Filter.Type) : x.Id != null)
        //        .Where(x => model.Filter.IsAvailable != null ? x.IsAvailable == Convert.ToBoolean(model.Filter.IsAvailable) : x.Id != null)
        //        .Skip((model.Page - 1) * model.ItemsPerPage)
        //        .Take(model.ItemsPerPage)
        //        .Select(x => new RoomIndexViewModel()
        //        {
        //            Id = x.Id,
        //            Capacity = x.Capacity,
        //            RoomType = x.RoomType,
        //            IsAvailable = x.IsAvailable,
        //            PricePerBedAdult = x.PricePerBedAdult,
        //            PricePerBedChild = x.PricePerBedChild,
        //            Number = x.Number,
        //        })
        //        .ToListAsync();
        //    model.ElementsCount = await this.context.Customers.CountAsync();
        //    return model;
        //}
    }
}
