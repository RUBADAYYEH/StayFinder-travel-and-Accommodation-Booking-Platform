namespace Domain.Entities
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; } = default!;
        public string HotelName { get; set; } = string.Empty;
        public string HotelDescription { get; set;} = string.Empty;
        public int RoomCount { get; set;} = 0;
        public string? ThumbnailUrl;
        public int StarRating { get; set;} = 0;
        List<Review>? Reviews { get; set;}
        List<Room> Rooms { get; set;} = new List<Room>();
        public string Address { get; set;} = string.Empty;
        public string City { get; set;} = string.Empty;


    }
}
