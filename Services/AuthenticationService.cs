using Models;
using Models.Dtos;
using Data;

using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AuthenticationService
    {
        private static AppDbContext _context;
        private static IConfiguration _configuration;
		private static TokenValidationParameters _tokenValidationParameters;

        public AuthenticationService(
            AppDbContext context,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters)
        {
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }		
        
        public string RandomStringGeneration(int length)
		{
			var random = new Random();
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";

			return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}

        public async Task<AuthResult> GenerateJwtToken(IdentityUser user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

			// token descriptor
			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new []
				{
					new Claim("Id", user.Id),
					new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
				}),
                Issuer = "Collectors",
                Audience="Collector",
				Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value)),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};

			var token = jwtTokenHandler.CreateToken(tokenDescriptor);
			var jwtToken = jwtTokenHandler.WriteToken(token);

			var refreshToken = new RefreshToken()
			{
				JwtId = token.Id,
				Token = RandomStringGeneration(23), // generate a refresh token
				AddedDate = DateTime.UtcNow,
				ExpiryDate = DateTime.UtcNow.AddMinutes(30),
				IsRevoked = false,
				IsUsed = false,
				UserId = user.Id
			};

			await _context.RefreshTokens.AddAsync(refreshToken);
			await _context.SaveChangesAsync();

			return new AuthResult()
			{
				Result = true,
				RefreshToken = refreshToken.Token,
				Token = jwtToken
			};
		}

        public async Task<AuthResult> VerifyAndGenerateToken(TokenRequestDto tokenRequestDto, UserManager<IdentityUser> _userManager)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequestDto.Token, _tokenValidationParameters, out var validatedToken);

                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
                    
                    if(result == false)
                    {
                        return null;
                    }
                }

                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                if(expiryDate < DateTime.Now)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Expired token"
                        }
                    };
                }

                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequestDto.RefreshToken);

                if(storedToken == null || storedToken.IsUsed || storedToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid Tokens"
                        }
                    };
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if(storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid Tokens"
                        }
                    };
                }

                if(storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Expired Tokens"
                        }
                    };
                }

                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception e)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Sever error",
                        e.ToString()
                    }
                };
            }
        }

        public async Task RemoveRefreshToken(TokenRequestDto tokenRequestDto)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequestDto.RefreshToken);
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0,0,0,0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
    }
}