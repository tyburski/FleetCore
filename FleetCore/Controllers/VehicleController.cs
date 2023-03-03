using FleetCore.Models;
using FleetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetCore.Controllers
{
    [ApiController]
    [Route("api/vehicle")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            return Ok(_vehicleService.GetById(id));
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_vehicleService.GetAll());
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateVehicleModel model)
        {
            var id = _vehicleService.Create(model);

            return Created($"/api/vehicle/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateVehicleModel model)
        {
            var _id = _vehicleService.Update(id, model);
            return Ok($"/api/vehicle/{_id}");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var result = _vehicleService.Delete(id);
            if (result == false) return NoContent();
            else return Ok();
        }

        [HttpPost("{id}/event")]
        public ActionResult CreateEvent([FromRoute]int id, [FromBody]CreateEventModel model)
        {
            return Ok(_vehicleService.CreateEvent(id, model));
        }

        [HttpPost("{id}/repair")]
        public ActionResult CreateRepair([FromRoute] int id, [FromBody] CreateRepairModel model)
        {
            return Ok(_vehicleService.CreateRepair(id, model));
        }
        [HttpPost("refueling")]
        public ActionResult CreateRefueling([FromBody] CreateRefuelingModel model)
        {
            return Ok(_vehicleService.CreateRefueling(model));
        }
    }
}
