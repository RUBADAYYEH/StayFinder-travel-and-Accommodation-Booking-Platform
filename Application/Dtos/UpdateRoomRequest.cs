﻿using Domain.Entities.Enums;
using Domain.Entities;

namespace Application.Dtos
{
    public class UpdateRoomRequest
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public RoomType RoomType { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool IsPetFriendly { get; set; }
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
