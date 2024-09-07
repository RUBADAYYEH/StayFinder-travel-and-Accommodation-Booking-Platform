using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers
{
    [Route("owners")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Owner>>> GetOwners()
        {
            var owners = await _ownerService.GetAllAsync();
            return Ok(owners);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody] CreateOwnerRequest owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _ownerService.CreateOwnerAsync(owner);

            return CreatedAtAction(nameof(GetOwners), new { id = owner.OwnerId }, owner);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchOwner(Guid id, [FromBody] UpdateOwnerRequest updateRequest)
        {
            try
            {
                updateRequest.OwnerId = id;
                var result = await _ownerService.UpdateOwnerAsync(updateRequest);

                if (!result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the owner.");
                }

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Owner not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{ownerid}")]
        public async Task<ActionResult> DeleteOwner(Guid ownerid)
        {
            var owner = _ownerService.GetById(ownerid);
            if (owner == null)
            {
                return NotFound("Owner Not Found");
            }

            await _ownerService.DeleteOwnerAsync(ownerid);
            return NoContent();
        }
    }
}
