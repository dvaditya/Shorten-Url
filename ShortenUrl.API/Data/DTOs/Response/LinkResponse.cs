using System;

namespace ShortenUrl.API.Data.DTOs.Response
{
    public class LinkResponse
    {
        public string UserId { get; set; }
        public string Link { get; set; }
        public string Slug { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Expiry { get; set; }
    }
}
