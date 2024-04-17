using Castle.Core.Resource;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Clients;
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
	public class ClientsService : IClientsService
	{
		private readonly ApplicationDbContext context;

		public ClientsService(ApplicationDbContext context)
		{
			this.context = context;
		}
		public async Task<string> CreateClientAsync(ClientCreateViewModel model)
		{
			Client client = new Client()
			{
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				IsAdult = model.IsAdult,
				Number = model.PhoneNumber,
			};
			await this.context.Clients.AddAsync(client);
			await this.context.SaveChangesAsync();
			return client.Id;
		}
		public async Task<ClientsIndexViewModel> GetClientsAsync(ClientsIndexViewModel model)
		{
			if (model == null)
			{
				model = new ClientsIndexViewModel(0);
			}
			IQueryable<Client> dataClients = context.Clients;

			if (!string.IsNullOrWhiteSpace(model.FilterByName))
			{
				dataClients = dataClients.Where(x => x.FirstName.Contains(model.FilterByName) || x.LastName.Contains(model.FilterByName));
			}

			model.ElementsCount = await dataClients.CountAsync();

			model.Clients = await dataClients
				.Skip((model.Page - 1) * model.ItemsPerPage)
				.Take(model.ItemsPerPage)
				.Select(x => new ClientIndexViewModel()
				{
					Id = x.Id,
					Email = x.Email,
					FirstName = x.FirstName,
					LastName = x.LastName,
					IsAdult = x.IsAdult,
					PhoneNumber = x.Number,
				})
				.ToListAsync();
			return model;
		}
		public async Task<ClientDetailsViewModel> GetClientDetailsByIdAsync(string id)
		{
			Client client = await this.context.Clients.FindAsync(id);
			if (client != null)
			{
				ClientDetailsViewModel model = new ClientDetailsViewModel()
				{
					Email = client.Email,
					FirstName = client.FirstName,
					LastName = client.LastName,
					IsAdult = client.IsAdult,
					PhoneNumber = client.Number,
				};
                model.History = await context.ClientHistories
                  .Where(x => x.ClientId == client.Id)
                  .Select(x => new ClientHistoryViewModel()
                  {
                      ClientId = x.ClientId,
                      Price = x.ResPrice,
                      Client = x.Client,
                      AccomodationDate = x.AccomodationDate,
                      LeaveDate = x.LeaveDate,
                      RoomNumber = x.ResRoomNumber,
                  })
                   .ToListAsync();

                return model;
			}
			return null;
		}
		public async Task<ClientEditViewModel> EditCustomerByIdAsync(string id)
		{
			Client client = await this.context.Clients.FindAsync(id);
			if (client != null)
			{
				return new ClientEditViewModel()
				{
					Id = client.Id,
					Email = client.Email,
					FirstName = client.FirstName,
					LastName = client.LastName,
					IsAdult = client.IsAdult,
					PhoneNumber = client.Number,
				};
			}
			return null;
		}
		public async Task UpdateCustomerAsync(ClientEditViewModel model)
		{
			Client client = new Client()
			{
				Id = model.Id,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				IsAdult = model.IsAdult,
				Number = model.PhoneNumber,
			};
			context.Update(client);
			await context.SaveChangesAsync();
		}
        public async Task<ClientDetailsViewModel> DeleteClientByIdAsync(string id)
        {
            Client client = await context.Clients.FindAsync(id);
            if (client != null)
            {
                ClientDetailsViewModel model = new ClientDetailsViewModel()
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    IsAdult = client.IsAdult,
                    PhoneNumber = client.Number,
                };
                return model;
            }
            return null;
        }
        public async Task DeleteClientAsync(ClientDetailsViewModel model)
        {
			Client client = await context.Clients.FindAsync(model.Id);
			if (client != null)
			{
				//if (client.ReservationId != null)
				//{
				//	client.ReservationId= null;
				//}
				context.Clients.Remove(client);
				await this.context.SaveChangesAsync();
			}
		}
    }
}
