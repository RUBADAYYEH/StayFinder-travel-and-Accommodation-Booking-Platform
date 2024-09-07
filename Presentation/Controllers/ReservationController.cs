using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.Controllers
{
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IPaymentService _paymentService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReservationController(IReservationService reservationService, IPaymentService paymentService, IHttpContextAccessor contextAccessor)
        {
            _reservationService = reservationService;
            _paymentService = paymentService;
            _contextAccessor = contextAccessor;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
        {
            var res = await _reservationService.GetAsync();
            return Ok(res);
        }
        [HttpGet("unconfirmed")]
        public ActionResult<CreateReservationRequest> GetUnconfirmed()
        {
            var reservations = _contextAccessor.HttpContext!.Session.GetString("Reservation");
            if (reservations == null)
            {
                return NotFound("No unconfirmed reservations found.");
            }
            var res = JsonConvert.DeserializeObject<CreateReservationRequest>(reservations);
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
            var PricePerNight = await _reservationService.GetRoomPricePerNight(reservation.RoomId);
            var TotalFees = ((reservation.CheckOutDate - reservation.CheckInDate).Days) * PricePerNight;

            var payment = await _paymentService.CreatePayment(TotalFees, "eur");

            reservation.PaymentId = payment.PaymentId;
            reservation.TotalFees = TotalFees;
            reservation.ReservationId = Guid.NewGuid();
            var reservationString = JsonConvert.SerializeObject(reservation);
            _contextAccessor.HttpContext!.Session.SetString("Reservation", reservationString);
            return CreatedAtAction(nameof(Get), new { reservationid = reservation.ReservationId }, new
            {
                reservation.ReservationId,
                PaymentId = payment.PaymentId,
                Status = payment.Status
            });
        }
        [HttpPost("{reservationId}/confirm")]
        public async Task<IActionResult> ConfirmBooking(Guid reservationId)
        {

            var reservations = _contextAccessor.HttpContext!.Session.GetString("Reservation");
            if (reservations == null)
            {
                return NotFound("No unconfirmed reservations found.");
            }
            var res = JsonConvert.DeserializeObject<CreateReservationRequest>(reservations);

            if (res == null || res.ReservationId != reservationId || res.ReservationId == null)
            {
                return NotFound("Reservation not found");
            }
            var paymentSuccess = await _paymentService.ProcessPayment(res.PaymentId!.Value);
            if (!paymentSuccess)
            {
                return BadRequest("Payment failed");
            }
            var finalres = new Reservation() { ReservationId = reservationId, PaymentId = res.PaymentId.GetValueOrDefault(), RoomId = res.RoomId, CheckInDate = res.CheckInDate, CheckOutDate = res.CheckOutDate, TotalFees = res.TotalFees!.GetValueOrDefault() };
            
            await _reservationService.ConfirmReservationAsync(finalres);
            HttpContext.Session.Remove("Reservation");

            return Ok("Booking confirmed");
        }
        [HttpDelete("{reservationid}")]
        public async Task<ActionResult> DeleteReservation(Guid reservationid)
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
