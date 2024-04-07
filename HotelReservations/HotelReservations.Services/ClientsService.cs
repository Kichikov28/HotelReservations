using Castle.Core.Resource;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Clients;
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
            model.Clients = await this.context.Clients
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
            model.ElementsCount = await this.context.Clients.CountAsync();
            return model;
        }
        public async Task<ClientDetailsViewModel> GetCustomerDetailsByIdAsync(string id)
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

                //model.GetHashC = await context.CustomerHistory
                //   .Where(x => x.CustomerId == client.Id)
                //   .Skip((model.Page - 1) * model.ItemsPerPage)
                //   .Take(model.ItemsPerPage)
                //   .Select(x => new CustomerHistoryViewModel()
                //   {
                //       CustomerId = x.CustomerId,
                //       ResPrice = x.ResPrice,
                //       Customer = x.Customer,
                //       ResAccomDate = x.ResAccomDate,
                //       ResLeaveDate = x.ResLeaveDate,
                //       ResRoomNumber = x.ResRoomNumber,
                //   })
                //    .ToListAsync();

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
    }
}
