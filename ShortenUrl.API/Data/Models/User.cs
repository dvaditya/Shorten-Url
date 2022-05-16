using System.ComponentModel.DataAnnotations;

namespace ShortenUrl.API.Data.Models
{
	public class User
	{
		[Key]
		public string Id { get; set; }
		public string SessionId { get; set; }
	}
}
