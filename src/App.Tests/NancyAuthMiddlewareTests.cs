namespace App.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Owin;
    using Owin.Testing;
    using Xunit;

    public class NancyAuthMiddlewareTests
    {
        private readonly HttpClient _client;

        public NancyAuthMiddlewareTests()
        {
            OwinTestServer testServer = OwinTestServer.Create(builder => new Startup().Configuration(builder));
            _client = new HttpClient(new OwinHttpMessageHandler(testServer.Invoke) { UseCookies = true });
        }

        [Fact]
        public async Task When_not_logged_in_and_get_claims_count_should_get_unauthorized()
        {
            HttpResponseMessage response = await _client.GetAsync("http://example.com/getclaimscount");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task When_log_in_fails_should_get_unauthorized()
        {
            HttpResponseMessage response = await _client.PostAsync(
                "http://example.com/login",
                new FormUrlEncodedContent(new Dictionary<string, string>
                                          {
                                              {"UserName", "damian"},
                                              {"Password", "wrongpassword"},
                                          }));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task When_log_in_succeeds_should_get_redirect_to_root()
        {
            HttpResponseMessage response = await _client.PostAsync(
                "http://example.com/login",
                new FormUrlEncodedContent(new Dictionary<string, string>
                                          {
                                              {"UserName", "damian"},
                                              {"Password", "password"},
                                          }));
            Assert.Equal(HttpStatusCode.SeeOther, response.StatusCode);
            Assert.True(response.Headers.Location.ToString() == "/");
        }

        [Fact]
        public async Task When_logged_in_and_get_claims_count_then_should_get_result()
        {
            await _client.PostAsync("http://example.com/login", new FormUrlEncodedContent(new Dictionary<string, string>
                                                                                          {
                                                                                              {"UserName", "damian"},
                                                                                              {"Password", "password"},
                                                                                          }));
            HttpResponseMessage responseMessage = await _client.GetAsync("http://example.com/getclaimscount");
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            var claimCount = int.Parse(await responseMessage.Content.ReadAsStringAsync());
            Assert.True(claimCount > 0);
        }
    }
}