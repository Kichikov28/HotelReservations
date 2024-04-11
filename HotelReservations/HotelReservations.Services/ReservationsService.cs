using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Resource;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Reservations;
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
				HasBreakfast = model.HasBreakfast

			};
			reservation.Clients = new List<Client>();
			

			//reservation.Price = CalculatePriceWithExtras(model.HasBreakfast, model.HasAllInclusive);

			//if (room.Id != reservation.RoomId)
			//{
			//	Reservation roomres = await context.Reservations.FirstOrDefaultAsync(x => x.RoomId == room.Id);
			//	if (roomres == null && room.IsAvailable)
			//	{
			//		ReserveRoom(room, reservation);
			//	}
			//}

			//Add Reservation to database and save
			await this.context.Reservations.AddAsync(reservation);
			await this.context.SaveChangesAsync();

			//foreach (var client in clients)
			//{
			//	Client client = await FindClientAsync(client);
			//	if (client.Reservation == null && room.Capacity > reservation.Clients.Count)
			//	{
			//		await AddCustomerToReservationAsync(client, reservation);
			//		reservation.Price += CalculatePrice(model.LeaveDate, model.AccommodationDate, room, client);
			//	}
			//}

			//Attach instance of Reservation
			context.Reservations.Attach(reservation);
			await this.context.SaveChangesAsync();
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
		private double CalculatePriceWithExtras(Boolean HasBreakfast, Boolean HasAllInclusive)
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
		//private async Task AddClientToReservationAsync(Client client,
		//	Reservation reservation)
		//{
		//	client.Reservation = reservation;

		//	CustomerHistory ch = new CustomerHistory()
		//	{
		//		Customer = client,
		//		ResAccomDate = reservation.AccommodationDate,
		//		ResLeaveDate = reservation.LeaveDate,
		//		ResPrice = reservation.Price,
		//		ResRoomNumber = reservation.Room.Number,
		//	};

		//	context.CustomerHistory.Add(ch);

		//	context.Reservations.Attach(reservation);
		//	await this.context.SaveChangesAsync();
		//}
		public async Task<int> GetRoomCapacityAsync(string id)
		{
			Room room = await context.Rooms.FindAsync(id);
			return room.Capacity;
		}
		public async Task<Client> FindCustomerAsync(Client clnt)
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
	}
}
