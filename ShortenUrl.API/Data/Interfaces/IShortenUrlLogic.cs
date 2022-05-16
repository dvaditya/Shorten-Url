using ShortenUrl.API.Data.DTOs.Request;
using ShortenUrl.API.Data.DTOs.Response;
using ShortenUrl.API.Data.Models;
using System.Collections.Generic;

namespace ShortenUrl.API.Data.Interfaces
{
    public interface IShortenUrlLogic
    {
        User GetUserProfile(string userId, string sessionId);
        List<LinkResponse> GetLinks(string userId);
        LinkResponse GetLinkBySlug(string slug);
        void SetLink(string userId, SetLinkRequest setLinkRequest);
        void UnsetLink(string userId, string slug);
    }
}
