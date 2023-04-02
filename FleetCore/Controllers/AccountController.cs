using FleetCore.Models;
using FleetCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FleetCore.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService) 
        {
            _accountService = accountService;
        }
        [HttpPost("create")]
        public ActionResult Create([FromBody]CreateUserModel model)
        {
            var result = _accountService.Create(model);
                           
            if (result.Result is false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginModel model)
        {
            var result = _accountService.Authenticate(model);
            if (result is null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        [HttpPost("validate")]
        public ActionResult Validate([FromBody]string userId)
        {
            var result = _accountService.Validate(userId);
            if (result.Result is true) return Ok();
            else return BadRequest();
        }
        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            return Ok(_accountService.GetAll());
            
        }
        [HttpPost("password")]
        public ActionResult ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (_accountService.ChangePassword(model).Result is true) return Ok();
            else return BadRequest();
        }
        [HttpPost("rpassword")]
        public ActionResult ResetPassword([FromBody]string fullname)
        {
            var result = _accountService.ResetPassword(fullname);
            if (result.Result is false) return BadRequest();
            else return Ok();
        }
        [HttpPost("delete")]
        public ActionResult Delete([FromBody] string fullname)
        {
            var result = _accountService.DeleteUser(fullname);
            if (result.Result is false) return BadRequest();
            else return NoContent();
        }
        [HttpPost("role")]
        public ActionResult ChangeRole([FromBody] string fullname)
        {
            var result = _accountService.ChangeRole(fullname);
            if (result.Result is false) return BadRequest();
            else return Ok();
        }

        [HttpGet("getlogs")]
        public ActionResult GetLogs()
        {
            return Ok(_accountService.GetLogs());
        }
        [HttpPost("getrefuelings")]
        public ActionResult GetRefuelings([FromBody] string fullname)
        {
            return Ok(_accountService.GetRefuelings(fullname));
        }
    }
}
