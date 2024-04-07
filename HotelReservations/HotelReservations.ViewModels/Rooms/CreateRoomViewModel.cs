using HotelReservations.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.ViewModels.Rooms
{
    public class CreateRoomViewModel
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }
        [Required]
        [Display(Name = "Room Type")]
        public RoomType RoomType { get; set; }
        [Required]
        [Display(Name = "Price for adult")]
        public double PricePerAdultBed { get; set; }
        [Required]
        [Display(Name = "Price for child")]
        public double PricePerChildBed { get; set; }
    }
}
