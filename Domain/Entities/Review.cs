namespace Domain.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int HotelId { get; set; }

    }
}
