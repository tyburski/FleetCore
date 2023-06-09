﻿using FleetCore.Models;
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

        [HttpPost("get")]
        public ActionResult GetByPlate([FromBody]string plate)
        {
            var result = _vehicleService.GetByPlate(plate);
            if (result is null) return NotFound();
            else return Ok(result);
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_vehicleService.GetAll());
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateVehicleModel model)
        {
            var result = await _vehicleService.Create(model);

            if(result is false)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("update")]
        public ActionResult Update([FromBody] UpdateVehicleModel model)
        {
            var result = _vehicleService.Update(model);
            if (result is true) return Ok();
            else return BadRequest();
        }
        [HttpPost("mileage")]
        public ActionResult UpdateMileage([FromBody] UpdateMileageModel model)
        {
            var result = _vehicleService.UpdateMileage(model);
            if (result is true) return Ok();
            else return BadRequest();
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody]string plate)
        {
            var result = _vehicleService.Delete(plate);
            if (result is false) return BadRequest();
            else return NoContent();
        }

        [HttpPost("repair/create")]
        public ActionResult CreateRepair([FromBody] CreateRepairModel model)
        {
            var result = _vehicleService.CreateRepair(model);
            if (result.Result is false) return BadRequest();
            else return Ok();
        }
        [HttpPost("repair/finish")]
        public ActionResult CreateRepair([FromBody] FinishRepairModel model)
        {
            var result = _vehicleService.FinishRepair(model);
            if (result.Result is false) return BadRequest();
            else return Ok();
        }
        [HttpPost("refueling")]
        public ActionResult CreateRefueling([FromBody]CreateRefuelingModel model)
        {
            var result = _vehicleService.CreateRefueling(model);
            if (result.Result is true)
            {
                return Ok();
            }
            else return BadRequest();
        }
        [HttpPost("event")]
        public ActionResult UpdateEvent([FromBody]UpdateEventDate model)
        {
            var result = _vehicleService.UpdateEvent(model);
            if (result.Result is true) return Ok();
            else return BadRequest();
        }
    }
}
