using Microsoft.AspNetCore.Mvc;
using ShortenUrl.API.Data.DTOs.Request;
using ShortenUrl.API.Data.Interfaces;
using System;

namespace ShortenUrl.API.Controllers
{
    [ApiController]
	public class ShortenUrlController : ControllerBase
	{
		private readonly IShortenUrlLogic _shortenUrlLogic;

        public ShortenUrlController(IShortenUrlLogic shortenUrlLogic)
        {
            _shortenUrlLogic = shortenUrlLogic;
        }

        [HttpGet("api/profile")]
		public IActionResult GetProfile()
        {
            try
            {
				return Ok(_shortenUrlLogic.GetUserProfile(Request.Cookies["userid"], Request.Cookies["sessionid"]));
			}
			catch (Exception)
            {
				return Unauthorized();
            }
        }

		[HttpGet("api/links")]
		public IActionResult GetLinks()
		{
            try
            {
				return Ok(_shortenUrlLogic.GetLinks(Request.Cookies["userid"]));
			}
			catch (Exception)
            {
				return BadRequest("Error occured while getting links for the user.");
			}
		}

		[HttpPost("api/link")]
		public IActionResult SetLink([FromBody]SetLinkRequest setLinkRequest)
		{
			try
			{
				_shortenUrlLogic.SetLink(Request.Cookies["userid"], setLinkRequest);

				return Ok(_shortenUrlLogic.GetLinks(Request.Cookies["userid"]));
			}
			catch (Exception)
			{
				return BadRequest("Error in converting url to short link.");
			}
		}

		[HttpDelete("api/unlink/{slug}")]
		public IActionResult DeleteSlug(string slug)
		{
			try
			{
				_shortenUrlLogic.UnsetLink(Request.Cookies["userid"], slug);

				return Ok(_shortenUrlLogic.GetLinks(Request.Cookies["userid"]));
			}
			catch (Exception)
			{
				return BadRequest("Error occured while deleting the link.");
			}
		}

		[HttpGet("api/slug/{id}")]
		public IActionResult GetLinkById(string id)
		{
			try
			{
				var link = _shortenUrlLogic.GetLinkBySlug(id);
				if(link == null)
                {
					return BadRequest("Unable to find the link.");
                }

				return Ok(link);
			}
			catch (Exception)
			{
				return BadRequest("Error occured while getting the link.");
			}
		}
	}
}
