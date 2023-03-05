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
            var result = _jWTManager.Authenticate(model);
            if (result is null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }
}
