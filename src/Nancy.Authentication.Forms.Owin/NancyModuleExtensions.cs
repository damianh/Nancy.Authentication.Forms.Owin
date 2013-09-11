namespace Nancy.Authentication.Forms.Owin
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public static class NancyModuleExtensions
    {
        private const string ServerUser = "server.User";

        public static ClaimsPrincipal GetClaimsPrincipal(this INancyModule module)
        {
            var environment = module.Context.Items[Nancy.Owin.NancyOwinHost.RequestEnvironmentKey] as IDictionary<string, object>;
            if (environment == null || !environment.ContainsKey(ServerUser))
            {
                return null;
            }
            return environment[ServerUser] as ClaimsPrincipal;
        }
    }
}