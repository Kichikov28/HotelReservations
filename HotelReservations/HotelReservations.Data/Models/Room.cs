namespace HotelReservations.Data.Models
{
    using HotelReservations.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Room
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Capacity { get; set; }
        public int Number { get; set; }
        public RoomType Type { get; set; }
        public bool IsAvailable { get; set; }
        public double PricePerAdultBed { get; set; }
        public double PricePerChildBed { get; set; }
        public string? ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }
    }
}
