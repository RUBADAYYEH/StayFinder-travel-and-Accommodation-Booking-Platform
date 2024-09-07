using Application.Abstraction;
using Application.Dtos;
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
    }
}
