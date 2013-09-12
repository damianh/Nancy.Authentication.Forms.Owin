namespace Nancy.Authentication.Forms.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Nancy.Cookies;

    public class NancyFormsAuthMiddleware
    {
        private const string ServerUser = "server.User";
        private readonly IClaimsPrincipalLookup _claimsPrincipalLookup;
        private readonly FormsAuthenticationConfiguration _formsAuthenticationConfiguration;
        private readonly Func<IDictionary<string, object>, Task> _next;

        public NancyFormsAuthMiddleware(Func<IDictionary<string, object>, Task> next,
            FormsAuthenticationConfiguration formsAuthenticationConfiguration,
            IClaimsPrincipalLookup claimsPrincipalLookup)
        {
            _next = next;
            _formsAuthenticationConfiguration = formsAuthenticationConfiguration;
            _claimsPrincipalLookup = claimsPrincipalLookup;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var requestHeaders = ((IDictionary<string, string[]>) environment["owin.RequestHeaders"]);
            if (!requestHeaders.ContainsKey("Cookie"))
            {
                await _next.Invoke(environment);
                return;
            }
            NancyCookie authCookie = GetFormsAuthCookies(requestHeaders["Cookie"]).SingleOrDefault();
            if (authCookie == null)
            {
                await _next.Invoke(environment);
                return;
            }
            string user = FormsAuthentication.DecryptAndValidateAuthenticationCookie(authCookie.Value,
                _formsAuthenticationConfiguration);
            Guid userId;
            if (Guid.TryParse(user, out userId))
            {
                ClaimsPrincipal claimsPrincipal = await _claimsPrincipalLookup.GetClaimsPrincial(userId);
                if (environment.ContainsKey(ServerUser))
                {
                    environment[ServerUser] = claimsPrincipal;
                }
                else
                {
                    environment.Add(ServerUser, claimsPrincipal);
                }
            }
            await _next.Invoke(environment);
        }

        private IEnumerable<NancyCookie> GetFormsAuthCookies(IEnumerable<string> cookieHeaders)
        {
            return cookieHeaders
                .Select(h => h.Split(';'))
                .Select(header => 
                    header.Select(c =>
                    {
                        string[] pair = c.Split('=');
                        return new NancyCookie(pair[0].Trim(), pair[1]);
                    })
                    .SingleOrDefault(c => c.Name == FormsAuthentication.FormsAuthenticationCookieName));
        }
    }
}