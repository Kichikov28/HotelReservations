using Castle.Core.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservations.Data.Models;

namespace HotelReservations.ViewModels.Clients
{
    public class ClientHistoryViewModel
    {
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public int RoomNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Accomodation date")]
        public DateTime AccomodationDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Leave date")]
        public DateTime LeaveDate { get; set; }

        [Display(Name = "Total price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
    }
}
