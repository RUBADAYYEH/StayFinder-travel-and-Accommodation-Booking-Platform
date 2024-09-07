using Application.Abstraction;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("featured")]
    public class FeaturedController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public FeaturedController(IWeatherService weatherService) {
            _weatherService = weatherService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Featured>>> Get()
        {
            var res= await _weatherService.GetSuggestedCitiesBasedOnCurrentTemp("Jerusalem");
            return Ok(res);
        }


    }
}
