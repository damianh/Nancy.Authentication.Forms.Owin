// ReSharper disable once CheckNamespace
namespace Nancy
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public static class NancyContextExtensions
    {
        private const string ServerUser = "server.User";

        public static ClaimsPrincipal GetClaimsPrincipal(this NancyContext context)
        {
            var environment = context.Items[Nancy.Owin.NancyOwinHost.RequestEnvironmentKey] as IDictionary<string, object>;
            if (environment == null || !environment.ContainsKey(ServerUser))
            {
                return null;
            }
            return environment[ServerUser] as ClaimsPrincipal;
        }
    }
}