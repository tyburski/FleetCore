using FleetCore.Models;
using FleetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetCore.Controllers
{
    [ApiController]
    [Route("api/organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService) 
        {
            _organizationService = organizationService;
        }
        [HttpPost("update")]
        public ActionResult Update([FromBody]UpdateOrganizationModel model)
        {
            var result = _organizationService.Update(model);
            if (result.Result is true) return Ok();
            else return BadRequest();
        }
        [HttpGet("get")]
        public ActionResult Get()
        {
            return Ok(_organizationService.Get());
        }
        [HttpPost("createnotice")]
        public ActionResult CreateNotice([FromBody]CreateNoticeModel model)
        {
            var result = _organizationService.CreateNotice(model);
            if (result.Result is false) return BadRequest();
            else return Ok();
        }

        [HttpGet("getnotices")]
        public ActionResult GetNotices()
        {
            return Ok(_organizationService.GetNotices());
        }
    }
}
