using Microsoft.AspNetCore.Mvc.Testing;
using ShortenUrl.API;
using Xunit;

namespace ShortenUrl.Tests.Helpers
{
    public abstract class TestBase : IClassFixture<FakeProgram<Startup, FakeStartup>>
    {
        protected WebApplicationFactory<FakeStartup> Factory { get; }

        public TestBase(FakeProgram<Startup, FakeStartup> factory)
        {
            Factory = factory;
        }
    }
}
