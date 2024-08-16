using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CreateReservationRequest
    {
        public int ReservationId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public decimal TotalFees { get; set; }
    }
}
