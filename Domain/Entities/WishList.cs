namespace Domain.Entities
{
    public class WishList
    {
        public Guid WishListid { get; set; }
        public Guid UserId { get; set; }
        public Guid HotelId { get; set; }
    }
}
