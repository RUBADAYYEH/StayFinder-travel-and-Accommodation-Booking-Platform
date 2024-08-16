using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("rooms")]
    public class RoomController : ControllerBase
    {
        IRoomService _roomService;
        IHotelService _hotelervice;
        public RoomController(IRoomService roomService,IHotelService hotelService)
        {
            _roomService = roomService;
            _hotelervice = hotelService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = _roomService.GetAllAsync();
            return Ok(rooms);
        }
        [HttpGet("{roomid}")]
        public async Task<ActionResult<Room>> GetRoomById(int roomid)
        {
            var room = await _roomService.GetById(roomid);
            if (room != null)
            {
                return Ok(room);
            }
            return NotFound();
        }
        /*  [HttpPost]
          public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest room)
          {
              if (!ModelState.IsValid)
              {
                  var errors = ModelState.Values
           .SelectMany(v => v.Errors)
           .Select(e => e.ErrorMessage)
           .ToList();

                  return BadRequest(new { errors });
              }
              await _roomService.CreateRoomAsync(room);

              return CreatedAtAction(nameof(GetRooms), new { id = room.RoomId }, room);
          }*/
        [HttpDelete("{roomid}")]
        public async Task<ActionResult> DeleteRoom(int hotelid,int roomid)
        {
            var room = _roomService.GetById(roomid);
            if (room == null)
            {
                return NotFound("Room Not Found");
            }
           
            await _roomService.DeleteRoomAsync(roomid);
            return NoContent();

        }
        [HttpPost("search")]
        public async Task<ActionResult<IQueryable<Room>>> SearchRooms([FromBody] SearchRoomRequest searchRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
                var results = await _roomService.SearchRoomsAsync(searchRequest);
                return Ok(results);
            
       
        }
     
    }
}
