﻿using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace Presentation.Controllers
{
    [Route("hotels")]
    [Authorize]
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
            var hotels = await _hotelService.GetAllAsync();
            return Ok(hotels);
        }
        [HttpGet("{hotelid}")]
        public async Task<ActionResult<Hotel>> GetHotelById(Guid hotelid)
        {
            var hotel = await _hotelService.GetById(hotelid);
            if (hotel != null)
            {
                return Ok(hotel);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{hotelid}")]
        public async Task<ActionResult> DeleteHotel(Guid hotelid)
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
        public async Task<ActionResult<IEnumerable<Room>>> GetAllrooms(Guid hotelid)
        {
            if (await _hotelService.GetById(hotelid) == null)
            {
                return NotFound("Hotel Not Found");
            }
            var rooms = await _hotelService.GetRoomsForHotelId(hotelid);
            return Ok(rooms);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("{hotelid}/rooms")]
        public async Task<IActionResult> CreateRoom(Guid hotelid, [FromBody] CreateRoomRequest room)
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
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchHotel(Guid id, [FromBody] UpdateHotelRequest updateRequest)
        {
            updateRequest.HotelId = id;
            var result = await _hotelService.UpdateHotelAsync(updateRequest);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the hotel.");
            }
            return NoContent();
        }
    }
}
