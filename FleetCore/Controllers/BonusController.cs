using FleetCore.Models;
using FleetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetCore.Controllers
{
    [ApiController]
    [Route("api/bonus")]
    public class BonusController : ControllerBase
    {
        private readonly IBonusService _bonusService;

        public BonusController(IBonusService bonusService)
        {
            _bonusService = bonusService;
        }
        [HttpPost]
        public ActionResult Get([FromBody]SearchBonusModel model) 
        {
            var result = _bonusService.Get(model);
            if(result is null) return NoContent();
            else return Ok(result);
        }
        [HttpPost("create")]
        public ActionResult Create([FromBody]CreateBonusModel model)
        {
            return Ok(_bonusService.Create(model));
        }
    }
}
