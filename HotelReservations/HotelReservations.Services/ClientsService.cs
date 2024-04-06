using Castle.Core.Resource;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Services
{
    public class ClientsService:IClientsService
    {
        private readonly ApplicationDbContext context;

        public ClientsService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<string> CreateCustomerAsync(ClientCreateViewModel model)
        {
            Client client = new Client()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsAdult = model.IsAdult,
                Number = model.PhoneNumber,
                ReservationId = model.ReservationId
            };
            await this.context.Clients.AddAsync(client);
            await this.context.SaveChangesAsync();
            return client.Id;
        }
    }
}
