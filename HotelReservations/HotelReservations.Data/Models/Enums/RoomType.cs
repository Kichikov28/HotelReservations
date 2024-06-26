﻿namespace HotelReservations.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;
    public enum RoomType
    {
        [Display(Name = "Two beds")]
        TwoSingleBeds,
        [Display(Name = "Apartment")]
        Apartment,
        [Display(Name = "Double bed")]
        DoubleBed,
        [Display(Name = "Penthouse")]
        PentHouse,
        Мaisonette
    }
}
