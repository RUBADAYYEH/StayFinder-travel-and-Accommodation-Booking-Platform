using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class CreateReservationRequest
    {
        public Guid PaymentId { get; set; }
        public Guid ReservationId { get; set; }
        [Required]
        public Guid RoomId { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public decimal TotalFees { get; set; }
    }
}
