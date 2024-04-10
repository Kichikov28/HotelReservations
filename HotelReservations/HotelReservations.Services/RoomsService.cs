using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Data.Models.Enums;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Clients;
using HotelReservations.ViewModels.Rooms;
using HotelReservations.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IndexRoomsViewModel> GetRoomsAsync(IndexRoomsViewModel model)
        {
            if (model == null)
            {
                model = new IndexRoomsViewModel(0);
            }
            IQueryable<Room> dataRooms = context.Rooms;

            if (model?.Capacity > 0)
            {
                dataRooms = dataRooms.Where(x => x.Capacity == model.Capacity);
            }
            if (!string.IsNullOrEmpty(model?.FilterByType))
            {
                dataRooms = dataRooms.Where(x => x.Type == Enum.Parse<RoomType>(model.FilterByType));
            }

            if (!string.IsNullOrEmpty(model?.IsAvailable.ToString()))
            {
                bool isAvailable = Convert.ToBoolean(model.IsAvailable);
                dataRooms = dataRooms.Where(x => x.IsAvailable == isAvailable);
            }

            model.Rooms = await dataRooms
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .Select(room => new IndexRoomViewModel()
                {
                    Id = room.Id,
                    Capacity = room.Capacity,
                    Number = room.Number,
                    RoomType = room.Type,
                    IsAvailable = room.IsAvailable,
                    PricePerAdultBed = room.PricePerAdultBed,
                    PricePerChildBed = room.PricePerChildBed,
                }).ToListAsync();

            model.ElementsCount = await context.Rooms.CountAsync();
            return model;
        }
        public async Task<RoomDetailsViewModel> GetRoomDetailsAsync(string id)
        {

            Room room = await this.context.Rooms.FindAsync(id);
            if (room != null)
            {
                RoomDetailsViewModel model = new RoomDetailsViewModel()
                {
                    Id = room.Id,
                    Capacity = room.Capacity,
                    IsAvailable = room.IsAvailable,
                    Number = room.Number,
                    PricePerAdultBed = room.PricePerAdultBed,
                    PricePerChildBed = room.PricePerChildBed,
                    RoomType = room.Type,
                };
                return model;
            }
            return null;
        }
        public async Task<EditRoomViewModel> EditRoomAsync(string id)
        {
            Room room = await context.Rooms.FindAsync(id);
            if (room != null)
            {
                return new EditRoomViewModel()
                {
                    Id = room.Id,
                    Capacity = room.Capacity,
                    IsAvailable = room.IsAvailable,
                    Number = room.Number,
                    PricePerAdultBed = room.PricePerAdultBed,
                    PricePerChildBed = room.PricePerChildBed,
                    RoomType = room.Type,
                };
            }
            return null;
        }
        public async Task<string> UpdateRoomAsync(EditRoomViewModel model)
        {
            Room room = new Room()
            {
                Id = model.Id,
                Capacity = model.Capacity,
                IsAvailable = model.IsAvailable,
                Number = model.Number,
                PricePerAdultBed = model.PricePerAdultBed,
                PricePerChildBed = model.PricePerChildBed,
                Type = model.RoomType,
            };
            context.Update(room);
            await context.SaveChangesAsync();
            return room.Id;
        }
    }
}
