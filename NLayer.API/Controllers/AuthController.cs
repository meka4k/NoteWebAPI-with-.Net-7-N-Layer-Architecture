using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Models;
using NLayer.Service.Services;

namespace NLayer.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register(LoginUser loginUser)
		{
			if(await _authService.RegisterUser(loginUser))
			return Ok("Succesfuly Done");

			return BadRequest("Something went wrong");
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginUser loginUser)
		{
			if(!ModelState.IsValid)
				return BadRequest();

			if (await _authService.Login(loginUser))
			{
				var tokenString = _authService.GenerateTokenString(loginUser);

				return Ok(tokenString);
			}
				

			return BadRequest();

		}


	}
}
