using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Resource;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Clients;
using HotelReservations.ViewModels.Reservations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelReservations.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly ApplicationDbContext context;

        public ReservationsService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<CreateReservationViewModel> CreateReservationAsync(CreateReservationViewModel model)
        {
            User user = await context.Users.FindAsync(model.UserId);
            Room room = await context.Rooms.FindAsync(model.RoomId);

            List<Client> clients = model.Clients
                .Select(x => new Client
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Number = x.Number,
                }).ToList();

            Reservation reservation = new Reservation()
            {
                User = user,
                AccommodationDate = model.AccommodationDate,
                LeaveDate = model.LeaveDate,
                HasAllInclusive = model.HasAllInclusive,
                HasBreakfast = model.HasBreakfast,
                Price = CalculatePriceWithExtra(model.HasBreakfast, model.HasAllInclusive)

            };

            if (room.IsAvailable)
            {
                ReserveRoom(room, reservation);
            }

            await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();

            foreach (var client in model.Clients)
            {
                Client client1 = new Client
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Number = client.Number,
                };

                reservation.Clients.Add(client);
                reservation.Price +=CalculatePrice(model.LeaveDate, model.AccommodationDate, room, client);
            }
            await context.SaveChangesAsync();
            return model;
        }
        public async Task<IndexReservationsViewModel> GetReservationsAsync(IndexReservationsViewModel model)
        {
            if (model == null)
            {
                model = new IndexReservationsViewModel(0);
            }

            model.ElementsCount = await context.Reservations.CountAsync();

            model.Reservations = await context.Reservations
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .Select(x => new IndexReservationViewModel()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Room = context.Rooms.FirstOrDefault(y => y.Id == x.RoomId),
                    AccommodationDate = x.AccommodationDate,
                    LeaveDate = x.LeaveDate,
                    HasAllInclusive = x.HasAllInclusive,
                    HasBreakfast = x.HasBreakfast,
                    Price = x.Price,
                })
                .ToListAsync();
            return model;
        }
        //public async Task<EditReservationViewModel> EditReservationByIdAsync(string id)
        //{
        //    Reservation reservation = await context.Reservations.FindAsync(id);
        //    if (reservation != null)
        //    {
        //        EditReservationViewModel model = new EditReservationViewModel()
        //        {
        //            Id = reservation.Id,
        //            UserId = reservation.UserId,
        //            RoomId = reservation.RoomId,
        //            AccommodationDate = reservation.AccommodationDate,
        //            LeaveDate = reservation.LeaveDate,
        //            HasAllInclusive = reservation.HasAllInclusive,
        //            HasBreakfast = reservation.HasBreakfast,
        //        };
        //        model.CustomersToRemove = reservation.Clients.Select(x => new ClientIndexViewModel()
        //        {
        //            Id = x.Id,
        //            Email = x.Email,
        //            FirstName = x.FirstName,
        //            IsAdult = x.IsAdult,
        //            LastName = x.LastName,
        //            PhoneNumber = x.Number,
        //        }).ToList();

        //        SelectList selectList = new SelectList(await GetAllRoomsSelectListAsync(model), "Id", "Number");
        //        model.Rooms = selectList;

        //        if (!String.IsNullOrEmpty(model.RoomId) && await GetRoomCapacityAsync(model.RoomId) > 0)
        //        {
        //            model.SelectedRoomCap = await GetRoomCapacityAsync(model.RoomId);

        //            for (int i = 0; i < model.SelectedRoomCap - reservation.Clients.Count; i++)
        //            {
        //                model.ClientsToAdd.Add(new Client());
        //            }

        //        }
        //        return model;
        //    }
        //    return null;
        //}
        private double CalculatePrice(DateTime leaveDate, DateTime accomdate, Room room,
   Client client)
        {
            double count = (leaveDate - accomdate).TotalDays;

            double price = 0;

            Client clientType = context.Clients.Find(client.Id);
            if (clientType.IsAdult)
            {
                price += room.PricePerAdultBed * count;
            }
            else
            {
                price += room.PricePerChildBed * count;
            }
            return price;
        }
        private double CalculatePriceWithExtra(Boolean HasBreakfast, Boolean HasAllInclusive)
        {
            double price = 0;
            if (HasBreakfast)
            {
                price += 135;
            }
            if (HasAllInclusive)
            {
                price += 280;
            }
            return price;
        }
        public bool HasReservationPassed(DateTime LeaveDate)
        {
            if (LeaveDate <= DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public async Task<int> GetRoomCapacityAsync(string id)
        {
            Room room = await context.Rooms.FindAsync(id);
            return room.Capacity;
        }
        public async Task<Client> FindClientAsync(Client clnt)
        {
            Client client = await context.Clients.FirstOrDefaultAsync(x => x.FirstName == clnt.FirstName &&
x.LastName == clnt.LastName && x.Number == clnt.Number);
            if (client != null)
            {
                return client;
            }
            else
            {
                return null;
            }
        }
        private static void ReserveRoom(Room room, Reservation reservation)
        {
            room.IsAvailable = false;
            if (reservation.Room != null)
            {
                reservation.Room.IsAvailable = true;
                reservation.Room.Reservation = null;
            }
            reservation.RoomId = room.Id;
            room.ReservationId = reservation.Id;
        }
    }
}
