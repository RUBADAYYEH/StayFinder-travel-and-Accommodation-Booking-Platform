namespace Domain.Entities
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid HotelId { get; set; }

    }
}
