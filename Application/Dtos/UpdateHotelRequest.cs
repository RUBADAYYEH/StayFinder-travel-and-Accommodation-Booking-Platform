namespace Application.Dtos
{
    public class UpdateHotelRequest
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string HotelDescription { get; set; } = string.Empty;
        public int RoomCount { get; set; } = 0;
        public string? ThumbnailUrl;
        public int StarRating { get; set; } = 0;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
