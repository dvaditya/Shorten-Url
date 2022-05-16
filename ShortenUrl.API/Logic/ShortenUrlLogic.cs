using Microsoft.AspNetCore.WebUtilities;
using ShortenUrl.API.Data;
using ShortenUrl.API.Data.DTOs.Request;
using ShortenUrl.API.Data.DTOs.Response;
using ShortenUrl.API.Data.Interfaces;
using ShortenUrl.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortenUrl.API.Logic
{
    public class ShortenUrlLogic: IShortenUrlLogic
    {
        private readonly AppDbContext _context;

        public ShortenUrlLogic(AppDbContext context)
        {
            _context = context;
        }

        public User GetUserProfile(string userId, string sessionId)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Id == userId && x.SessionId == sessionId);
            if (existingUser != null)
            {
                return existingUser;
            }

            var user = new User { Id = userId, SessionId = sessionId };
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public List<LinkResponse> GetLinks(string userId)
        {
            return _context.Links.Where(x => x.UserId == userId && x.Expiry >= DateTime.UtcNow)
                .Select(x => new LinkResponse
                {
                    UserId = x.UserId,
                    CreateDate = x.Created,
                    Expiry = x.Expiry,
                    Link = x.Url,
                    Slug = x.Slug
                }).ToList();
        }

        public LinkResponse GetLinkBySlug(string slug)
        {
            return _context.Links.Where(x => x.Id == GetIdFromSlug(slug))
                .Select(x => new LinkResponse
                {
                    UserId = x.UserId,
                    CreateDate = x.Created,
                    Expiry = x.Expiry,
                    Link = x.Url,
                    Slug = x.Slug
                }).FirstOrDefault();
        }

        public void SetLink(string userId, SetLinkRequest setLinkRequest)
        {
            var existingLink = _context.Links.FirstOrDefault(x => x.UserId == userId && x.Url == setLinkRequest.Link && x.Expiry >= DateTime.UtcNow);
            if (existingLink != null)
                return;

            var link = new Link {
                UserId = userId,
                Created = DateTime.UtcNow,
                Expiry = DateTime.UtcNow.AddHours(setLinkRequest.Expiry),
                Slug = string.Empty,
                Url = setLinkRequest.Link
            };
            using var transaction = _context.Database.BeginTransaction();
            try
            {   
                _context.Links.Add(link);
                _context.SaveChanges();

                UpdateSlug(GenerateSlugFromId(link.Id), link.Id);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void UnsetLink(string userId, string slug)
        {
            var existingSlug = _context.Links.FirstOrDefault(x => x.Slug == slug && x.UserId == userId);
            if(existingSlug != null)
            {
                _context.Links.Remove(existingSlug);
                _context.SaveChanges();
            }
        }

        #region private functions
        private void UpdateSlug(string slug, int linkId)
        {
            var link = _context.Links.FirstOrDefault(x => x.Id == linkId);
            if(slug != null)
            {
                link.Slug = slug;
                _context.SaveChanges();
            }
        }

        private static string GenerateSlugFromId(int linkId)
        {
            return WebEncoders.Base64UrlEncode(BitConverter.GetBytes(linkId));
        }

        private static int GetIdFromSlug(string slug)
        {
            return BitConverter.ToInt32(WebEncoders.Base64UrlDecode(slug));
        }
        #endregion
    }
}
