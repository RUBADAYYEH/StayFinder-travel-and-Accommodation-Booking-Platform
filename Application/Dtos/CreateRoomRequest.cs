using Domain.Entities.Enums;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CreateRoomRequest
    {
        public int RoomId { get; set; }
        [Required]
        public int HotelId { get; set; }
        [Required]
        public RoomType RoomType { get; set; }
        [Required]
        public int NumberOfAdults { get; set; }
        [Required]
        public int NumberOfChildren { get; set; }
        public bool IsPetFriendly { get; set; }
        [Required]
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
