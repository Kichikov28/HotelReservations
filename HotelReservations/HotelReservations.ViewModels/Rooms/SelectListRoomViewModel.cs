using HotelReservations.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Rooms
{
    public class SelectListRoomViewModel
    {
        public string Id { get; set; }
        public int Capacity { get; set; }
        public int Number { get; set; }
        public RoomType RoomType { get; set; }
        public double PricePerAdultBed { get; set; }
        public double PricePerChildBed { get; set; }
    }
}
