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

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
        {
            var res = await _reservationService.GetAsync();
            return Ok(res);
        }

        [HttpGet("{reservationid}")]
        public async Task<IActionResult> Get(int reservationid)
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
            await _reservationService.CreateReservationAsync(reservation);

            return CreatedAtAction(nameof(Get), new { reservationid = reservation.ReservationId }, reservation);
        }
        [HttpDelete("{reservationid}")]
        public async Task<ActionResult> DeleteRoom(int reservationid )
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
