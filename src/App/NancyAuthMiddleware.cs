namespace App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Nancy.Authentication.Forms;
    using Nancy.Cookies;
    using Owin;

    public class NancyAuthMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly FormsAuthenticationConfiguration _formsAuthenticationConfiguration;
        private readonly IUserManager _userManager;
        private const string ServerUser = "server.User";

        public NancyAuthMiddleware(Func<IDictionary<string, object>, Task> next,
            FormsAuthenticationConfiguration formsAuthenticationConfiguration, 
            IUserManager userManager)
        {
            _next = next;
            _formsAuthenticationConfiguration = formsAuthenticationConfiguration;
            _userManager = userManager;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var requestHeaders = ((IDictionary<string, string[]>) environment["owin.RequestHeaders"]);
            if (!requestHeaders.ContainsKey("Cookie"))
            {
                return _next.Invoke(environment);
            }
            NancyCookie authCookie = requestHeaders["Cookie"]
                .Select(c =>
                        {
                            var pair = c.Split('=');
                            return new NancyCookie(pair[0], pair[1]);
                        })
                .SingleOrDefault(c => c.Name == FormsAuthentication.FormsAuthenticationCookieName);
            string userId = FormsAuthentication.DecryptAndValidateAuthenticationCookie(authCookie.Value, _formsAuthenticationConfiguration);
            ClaimsPrincipal claimsPrincipal = _userManager.GetClaimsPrincial(Guid.Parse(userId));
            if (environment.ContainsKey(ServerUser))
            {
                environment[ServerUser] = claimsPrincipal;
            }
            else
            {
                environment.Add(ServerUser, claimsPrincipal);
            }
            return _next.Invoke(environment);
        }
    }
}