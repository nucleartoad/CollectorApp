using Configurations;
using Models;
using Models.Dtos;

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration;
		// private readonly JwtConfig _jwtConfig;

		public AuthenticationController (
			UserManager<IdentityUser> userManager,
			IConfiguration configuration)
			//JwtConfig jwtConfig)
		{
			_userManager = userManager;
			_configuration = configuration;
			// _jwtConfig = jwtConfig;
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
						// generate token
						var token = GenerateJwtToken(newUser);

						return Ok(new AuthResult()
						{
							Result = true,
							Token = token
						});
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
						var token = GenerateJwtToken(user);

						return Ok(new AuthResult()
						{
							Result = true,
							Token = token
						});
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

		private string GenerateJwtToken(IdentityUser user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

			// token descriptor
			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new []
				{
					new Claim("Id", user.Id),
					new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
				}),

				Expires = DateTime.Now.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};

			var token = jwtTokenHandler.CreateToken(tokenDescriptor);
			var jwtToken = jwtTokenHandler.WriteToken(token);

			return jwtToken;
		}
	}
}