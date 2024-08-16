using Application.Abstraction;
using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("owners")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnerController (IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Owner>>> GetOwners()
        {
            var owners = _ownerService.GetAllAsync().Result;
            return Ok(owners);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOwnerl([FromBody] CreateOwnerRequest owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _ownerService.CreateOwnerAsync(owner);

            return CreatedAtAction(nameof(GetOwners), new { id = owner.OwnerId }, owner);
        }
    }
}
