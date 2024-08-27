using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IPaymentService _paymentService;

        public ReservationController(IReservationService reservationService, IPaymentService paymentService)
        {
            _reservationService = reservationService;
            _paymentService = paymentService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
        {
            var res = await _reservationService.GetAsync();
            return Ok(res);
        }

        [HttpGet("{reservationid}")]
        public async Task<IActionResult> Get(Guid reservationid)
        {
            var res = await _reservationService.GetReservationDetailsByIdAsync(reservationid);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

           
            var totalAmount = reservation.TotalFees;

            var payment = _paymentService.CreatePayment(totalAmount, "usd");

            reservation.PaymentId = payment.PaymentId; 
            await _reservationService.CreateReservationAsync(reservation);

            return CreatedAtAction(nameof(Get), new { reservationid = reservation.ReservationId }, new
            {
                reservation.ReservationId,
                PaymentId = payment.PaymentId,
                Status = payment.Status
            });
        }
        [HttpPost("{reservationId}/confirm")]
        public IActionResult ConfirmBooking(Guid reservationId, [FromBody] Reservation request)
        {
            // Process the payment
            var paymentSuccess = _paymentService.ProcessPayment(request.PaymentId);

            if (!paymentSuccess)
            {
                return BadRequest("Payment failed");
            }

            _reservationService.ConfirmReservationAsync(reservationId);

            return Ok("Booking confirmed");
        }
        [HttpDelete("{reservationid}")]
        public async Task<ActionResult> DeleteReservation(Guid reservationid )
        {
            var res = _reservationService.GetReservationDetailsByIdAsync(reservationid);
            if (res == null)
            {
                return NotFound("Reservation Not Found");
            }
            await _reservationService.DeleteReservationAsync(reservationid);
            return NoContent();
        }
    }
}
