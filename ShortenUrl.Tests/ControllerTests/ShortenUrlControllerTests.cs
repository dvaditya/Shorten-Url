using Newtonsoft.Json;
using ShortenUrl.API;
using ShortenUrl.API.Data.Models;
using ShortenUrl.Tests.Helpers;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ShortenUrl.Tests.ControllerTests
{
    public class ShortenUrlControllerTests: TestBase
    {
        public ShortenUrlControllerTests(FakeProgram<Startup, FakeStartup> factory): base(factory) { }

        [Theory]
        [InlineData("User-1", "Session-1")]
        [InlineData("User-2", "Session-2")]
        [InlineData("User-3", "Session-3")]
        public async Task GetProfile_Should_Get_Existing_Profile_Or_Return_Newly_Created_Profile(string userId, string sessionId)
        {
            var client = Factory.CreateClient();
            client.DefaultRequestHeaders.Add("Cookie", $"sessionid={sessionId}; userid={userId}");

            var response = await client.GetAsync("/api/profile");
            response.EnsureSuccessStatusCode();

            var data = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.Equal(userId, data.Id);
            Assert.Equal(sessionId, data.SessionId);
        }

        [Theory]
        [InlineData("User-1", "Session-1")]
        [InlineData("User-2", "Session-2")]
        [InlineData("User-3", "Session-3")]
        public async Task GetProfile_Should_Get_Existing_Profile_Or_Return_Newly_Created_Profile(string userId, string sessionId)
        {
            var client = Factory.CreateClient();
            client.DefaultRequestHeaders.Add("Cookie", $"sessionid={sessionId}; userid={userId}");

            var response = await client.GetAsync("/api/profile");
            response.EnsureSuccessStatusCode();

            var data = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.Equal(userId, data.Id);
            Assert.Equal(sessionId, data.SessionId);
        }
    }
}
