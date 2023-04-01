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
        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateOrganizationModel model)
        {
            return Ok(_organizationService.Update(model));
        }
        [HttpGet("get")]
        public ActionResult Get()
        {
            return Ok(_organizationService.Get());
        }



    }
}
