using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
	public class UserLoginRequestDto
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}