using Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class SearchRoomRequest
    {
        
        public RoomType? RoomType { get; set; }
        [Required]
        public DateTime? CheckInDate { get; set; }
        [Required]
        public DateTime? CheckOutDate { get; set; }
        public string? City { get; set; }
        public string? HotelName { get; set; }
        [Range(0, int.MaxValue)]
        public int? NumberOfChildren { get; set; } = 0;
        [Range(1, int.MaxValue)]
        public int? NumberOfAdults { get; set; } = 2;
        public decimal? MinPricePerNight { get; set; }
        public decimal? MaxPricePerNight { get; set; }
        public int NumberOfRooms { get; set; } = 1;
    }
}
