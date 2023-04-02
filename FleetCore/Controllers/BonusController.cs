using FleetCore.Models;
using FleetCore.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody]CreateBonusModel model)
        {
            var result = _bonusService.Create(model);
            if(result.Result is true)
            {
                return Ok();
            }
            return BadRequest();            
        }

        [HttpPost("get")]
        public ActionResult GetAll([FromBody]string fullname)
        {
            return Ok(_bonusService.GetAll(fullname));
        }
    }
}
