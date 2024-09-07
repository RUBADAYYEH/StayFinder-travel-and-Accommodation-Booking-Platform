using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class CreateHotelRequest
    {
        public int HotelId { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public string HotelName { get; set; } = string.Empty;
        public string HotelDescription { get; set; } = string.Empty;
        [Required]
        public int RoomCount { get; set; } = 0;
        public string? ThumbnailUrl;
        [Required]
        public int StarRating { get; set; } = 0;
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
    }
}
