using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelervice;
        public RoomController(IRoomService roomService, IHotelService hotelService)
        {
            _roomService = roomService;
            _hotelervice = hotelService;
        }
        [HttpGet]
        public  ActionResult<IEnumerable<Room>> GetRooms()
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
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchRoom(int id, [FromBody] UpdateRoomRequest updateRequest)
        {
            updateRequest.RoomId = id;
            var result = await _roomService.UpdateRoomAsync(updateRequest);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the room.");
            }
            return NoContent();
        }
        [HttpDelete("{roomid}")]
        public async Task<ActionResult> DeleteRoom(int hotelid, int roomid)
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
        public ActionResult<IQueryable<Room>> SearchRooms([FromBody] SearchRoomRequest searchRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var results = _roomService.SearchRoomsAsync(searchRequest);
            return Ok(results);
        }
    }
}
