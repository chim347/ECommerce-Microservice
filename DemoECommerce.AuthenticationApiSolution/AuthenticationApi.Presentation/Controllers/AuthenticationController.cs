using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUser userInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userInterface.Register(userDTO);
            return result.Flag ? Ok(result) : BadRequest(Request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userInterface.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(Request);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response>> GetUser(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid user id");
            }

            var user = await userInterface.GetUser(id);
            return user.Id > 0 ? Ok(user) : NotFound(Request);
        }
    }
}
