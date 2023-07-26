using Models;
using Models.Dtos;
using Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private static AuthenticationService _service;
		private readonly UserManager<IdentityUser> _userManager;
		// private readonly JwtConfig _jwtConfig;

		public AuthenticationController (
			AuthenticationService authenticationService,
			UserManager<IdentityUser> userManager)
			//JwtConfig jwtConfig)
		{
			_service = authenticationService;
			_userManager = userManager;
			// _jwtConfig = jwtConfig;
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet]
		[Route("GetUser")]
		public string GetUser()
		{
			return User.Identity.Name;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
		{
			// validate incoming request
			if(ModelState.IsValid)
			{
				// Check if the user already exists
				var user = await _userManager.FindByNameAsync(requestDto.Username);

				if(user == null)
				{
					// create a user
					var newUser = new IdentityUser()
					{
						UserName = requestDto.Username
					};
					
					var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);

					if (isCreated.Succeeded)
					{
						return Ok();
					}

					return BadRequest(new AuthResult()
					{
						Errors = new List<string>()
						{
							"Sever Error"
						},
						Result = false
					});
				}
				
				return BadRequest( new AuthResult()
				{
					Result = false,
					Errors = new List<string>()
					{
						"Username already exists"
					}
				});
			}

			return BadRequest();
		}

		[Route("Login")]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
		{
			if(ModelState.IsValid)
			{
				// check if the user exists
				var user = await _userManager.FindByNameAsync(requestDto.Username);

				if(user != null)
				{
					var isCorrect = await _userManager.CheckPasswordAsync(user, requestDto.Password);

					if(isCorrect)
					{
						return Ok(await _service.GenerateJwtToken(user));
					}

					return BadRequest(new AuthResult()
					{
						Errors = new List<string>()
						{
							"Password incorrect"
						},
						Result = false
					});
				}

				return BadRequest(new AuthResult()
				{
					Errors = new List<string>()
					{
						"User not found"
					},
					Result = false
				});
			}

			return BadRequest(new AuthResult()
			{
				Errors = new List<string>()
				{
					"Invalid Payload"
				},
				Result = false
			});
		}

		[HttpPost]
		[Route("Logout")]
		public async Task<IActionResult> Logout([FromBody] TokenRequestDto tokenRequestDto)
		{
			if(ModelState.IsValid)
			{
				await _service.RemoveRefreshToken(tokenRequestDto);
				return Ok("User has been logged out");
			}
			return BadRequest(new AuthResult()
			{
				Errors = new List<string>()
				{
					"Invalid parameters"
				},
				Result = false
			});
		}

		[HttpPost]
		[Route("RefreshToken")]
		public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequestDto)
		{
			if(ModelState.IsValid)
			{
				var result = await _service.VerifyAndGenerateToken(tokenRequestDto, _userManager);

				if(result != null)
				{
					return Ok(result);
				}

				return BadRequest(new AuthResult()
				{
					Errors = new List<string>()
					{
						"Invalid tokens"
					},
					Result = false
				});
			}

			return BadRequest(new AuthResult()
			{
				Errors = new List<string>()
				{
					"Invalid parameters"
				},
				Result = false
			});
		}
	}
}