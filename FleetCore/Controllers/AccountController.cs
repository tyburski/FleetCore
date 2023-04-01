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
        private readonly IJWTManager _jWTManager;

        public AccountController(IAccountService accountService, IJWTManager jWTManager) 
        {
            _accountService = accountService;
            _jWTManager = jWTManager;
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
        [HttpPost("validateToken")]
        public ActionResult ValidateToken([FromBody]string token)
        {
            var result = _jWTManager.ValidateToken(token);

            return Ok(result);
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
    }
}
