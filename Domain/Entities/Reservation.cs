namespace Domain.Entities
{
    public class Reservation
    {
        public Guid ReservationId { get; set; }
        public Guid PaymentId { get; set; }
        public Guid RoomId { get; set; }
        public Room room { get; set; } = default!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalFees { get; set; }
        
    }
}
