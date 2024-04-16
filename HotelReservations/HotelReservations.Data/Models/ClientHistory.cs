using Castle.Core.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Data.Models
{
    public class ClientHistory
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int ResRoomNumber { get; set; }
        public DateTime AccomodationDate { get; set; }
        public DateTime LeaveDate { get; set; }

        [Column(TypeName = "money")]
        public double ResPrice { get; set; }
    }
}
