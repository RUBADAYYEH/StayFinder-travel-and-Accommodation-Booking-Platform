using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [Route("hotels")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;

        public HotelController(IHotelService repo, IRoomService roomService)
        {
            _hotelService = repo;
            _roomService = roomService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = _hotelService.GetAllAsync().Result;
            return Ok(hotels);
        }
        [HttpGet("{hotelid}")]
        public async Task<ActionResult<Hotel>> GetHotelById(int hotelid)
        {
            var hotel = await _hotelService.GetById(hotelid);
            if (hotel != null)
            {
                return Ok(hotel);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelRequest hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _hotelService.CreateHotelAsync(hotel);

            return CreatedAtAction(nameof(GetHotels), new { id = hotel.HotelId }, hotel);
        }
        [HttpDelete("{hotelid}")]
        public async Task<ActionResult> DeleteHotel(int hotelid)
        {
            var hotel = _hotelService.GetById(hotelid);
            if (hotel == null)
            {
                return NotFound("Hotel Not Found");
            }
            
            await _hotelService.DeleteHotelAsync(hotelid);
            return NoContent();
        }
        [HttpGet("{hotelid}/rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAllrooms(int hotelid)
        {
            if (await _hotelService.GetById(hotelid) == null)
            {
                return NotFound("Hotel Not Found");
            }
            var rooms = await _hotelService.GetRoomsForHotelId(hotelid);
            return Ok(rooms);
          
        }
        [HttpPost("{hotelid}/rooms")]
        public async Task<IActionResult> CreateRoom(int hotelid,[FromBody] CreateRoomRequest room)
        {
            if (await _hotelService.GetById(hotelid) == null) 
            {
                return NotFound("Hotel Not Found");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();

                return BadRequest(new { errors });
            }
            await _roomService.CreateRoomAsync(room);

            return NoContent();
        }

        }
}
