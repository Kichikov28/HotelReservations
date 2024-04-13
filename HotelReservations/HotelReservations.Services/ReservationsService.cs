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


        public async Task<SelectList> GetFreeRooms()
        {
            List<Room> rooms = context.Rooms.Where(x => x.IsAvailable == true).ToList();
            return new SelectList(rooms, "Id", "Number");
        }

        public async Task<CreateReservationViewModel> CreateReservationAsync(CreateReservationViewModel model)
        {
            User user = await context.Users.FindAsync(model.UserId);
            Room room = await context.Rooms.FindAsync(model.RoomId);

            if (user == null || room == null)
                return null; // Return null if user or room is not found

            List<Client> selectedClients = await context.Clients.Select
                (
                x => new Client
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Number = x.Number
                }).ToListAsync();
            double price = 0;

            Reservation reservation = new Reservation()
            {
                User = user,
                Room = room,
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

            foreach (var client in selectedClients)
            {
                //price += CalculatePrice(model.LeaveDate, model.AccommodationDate, room, client);
                if (reservation.Clients.Count < room.Capacity)
                {
                    await AddClientToReservationAsync(client, reservation);
                }
            }
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
        private async Task AddClientToReservationAsync(Client client, Reservation reservation)
        {
            // Assuming you have a DbSet<Client> in your context named "Clients"
            // You can add the client to the database and associate it with the reservation
            context.Clients.Add(client);

            // Associate the client with the reservation
            reservation.Clients.Add(client);

            // Save changes to the database
            await context.SaveChangesAsync();
        }
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
            }
            reservation.RoomId = room.Id;
        }
    }
}
