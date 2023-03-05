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
        [HttpPost]
        public ActionResult Create([FromBody] CreateOrganizationModel model)
        {
            var result = _organizationService.Create(model);
            if (result is false) return NoContent();
            else return Ok();
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var result = _organizationService.GetAll();
            if (result is null) return NoContent();
            return Ok(result);
        }
        [HttpPost("search/{search}")]
        public ActionResult GetByName([FromBody]SearchModel model)
        {
            return Ok(model);
        }
        [HttpGet("search/{search}")]
        public ActionResult GetByName([FromRoute] string search)
        {
            var result = _organizationService.GetByName(search);
            if (result is null) return NoContent();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute]string id)
        {
            var result = _organizationService.GetById(id);
            if (result is null) return NoContent();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute]string id, [FromBody]UpdateOrganizationModel model)
        {
            return Ok(_organizationService.Update(id, model));
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]string id)
        {
            var result = _organizationService.Delete(id);
            if(result is false) return NoContent();
            return Ok();
        }


    }
}
