namespace Nancy.Authenticaton.Forms.Owin.NancyApp
{
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;

    public class AppBootstrapper : DefaultNancyBootstrapper
    {
        private readonly FormsAuthenticationConfiguration _formsAuthenticationConfiguration;
        private readonly IUserManager _userManager;

        public AppBootstrapper(FormsAuthenticationConfiguration formsAuthenticationConfiguration, IUserManager userManager)
        {
            _formsAuthenticationConfiguration = formsAuthenticationConfiguration;
            _userManager = userManager;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            FormsAuthentication.Enable(pipelines, _formsAuthenticationConfiguration);
            container.Register(_userManager);
        }
    }
}