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
            var result = _accountService.Login(model);
            if (result.Result is false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            _accountService.Logout();
            return Ok();
        }
    }
}
