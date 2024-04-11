using HotelReservations.ViewModels.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Services.Contracts
{
    public interface IReservationsService
    {
        public Task<IndexReservationsViewModel> GetReservationsAsync(IndexReservationsViewModel model);
    }
}
