namespace Owin
{
    using App;
    using Nancy.Authentication.Forms;

    public static class NancyAuthMiddewareExtensions
    {
        public static IAppBuilder UseNancyAuth(this IAppBuilder builder, FormsAuthenticationConfiguration formsAuthenticationConfiguration, IUserManager userManager)
        {
            builder.Use(typeof(NancyAuthMiddleware), new object[] { formsAuthenticationConfiguration, userManager });
            return builder;
        }
    }
}