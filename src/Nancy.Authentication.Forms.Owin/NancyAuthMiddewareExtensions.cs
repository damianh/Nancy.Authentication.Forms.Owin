// ReSharper disable once CheckNamespace
namespace Owin
{
    using Nancy.Authentication.Forms;
    using Nancy.Authentication.Forms.Owin;

    public static class NancyAuthMiddewareExtensions
    {
        public static IAppBuilder UseNancyAuth(this IAppBuilder builder, FormsAuthenticationConfiguration formsAuthenticationConfiguration, IClaimsPrincipalLookup claimsPrincipalLookup)
        {
            builder.Use(typeof(NancyAuthMiddleware), new object[] { formsAuthenticationConfiguration, claimsPrincipalLookup });
            return builder;
        }
    }
}