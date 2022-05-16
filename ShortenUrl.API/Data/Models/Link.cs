using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortenUrl.API.Data.Models
{
	public class Link
	{
		[Key]
		public int Id { get; set; }
		public string Url { get; set; }
		public string Slug { get; set; }
		public DateTime Created { get; set; }
		public DateTime Expiry { get; set; }
		[ForeignKey("User")]
		public string UserId { get; set; }

		public User User { get; set; }
	}
}
