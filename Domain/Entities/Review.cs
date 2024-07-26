namespace Domain.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int HotelId { get; set; }
        
    }
}
