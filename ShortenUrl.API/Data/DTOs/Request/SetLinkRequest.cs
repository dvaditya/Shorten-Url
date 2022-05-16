namespace ShortenUrl.API.Data.DTOs.Request
{
    public class SetLinkRequest
    {
        public string Link { get; set; }
        public double Expiry { get; set; }
    }
}
