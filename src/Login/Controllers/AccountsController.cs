using Login.DataContext;
using Login.Identity;
using Login.Models;
using Login.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        private readonly JwtHandler _jwtHandler;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(ILogger<AccountsController> logger, ApplicationContext context,
            JwtHandler jwtHandler, UserManager<ApplicationUser> userManager) : base(logger, context)
        {
            _jwtHandler = jwtHandler;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == model.Username);
            //var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return NotFound();

            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (checkPassword)
            {
                if (user.IsDeleted == true)
                {
                    return BadRequest(new { message = Message.ACCOUNT_BLOCKED });
                }

                _logger.LogInformation(LogAction.Login.ToString(), user.Id);

                string token = await _jwtHandler.GenerateToken(user);

                return Ok(new { token });
            }

            return BadRequest(new { message = Message.LOGIN_FAILED });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation(LogAction.Logout.ToString());

            return Ok();
        }

    }
}
