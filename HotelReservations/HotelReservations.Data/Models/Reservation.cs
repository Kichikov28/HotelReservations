using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Data.Models
{
    public class Reservation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string RoomId { get; set; }
        public virtual Room Room { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
