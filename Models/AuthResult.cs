namespace Models
{
	public class AuthResult
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
		public Boolean Result { get; set; }
		public List<string> Errors { get; set; }
	}
}