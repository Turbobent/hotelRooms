﻿namespace hotelRooms.Models
{
    public class Rooms
    {
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public int Price { get; set; }
        public bool occipied { get; set; }
        public int maxPersoncount { get; set; }
        public DateOnly startdate { get; set; }
        public DateOnly slutdate { get; set; }
        public int temp { get; set; }
    }
}