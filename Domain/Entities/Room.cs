using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;
        public RoomType RoomType { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public bool IsPetFriendly { get; set; }
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public List<Reservation>? Reservations { get; set; }

    }
}
