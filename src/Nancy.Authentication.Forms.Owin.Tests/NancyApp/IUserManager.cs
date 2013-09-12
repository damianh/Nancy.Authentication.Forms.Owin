namespace Nancy.Authenticaton.Forms.Owin.NancyApp
{
    using System;

    public interface IUserManager
    {
        bool Authenticate(string userName, string password, out Guid identifier);
    }
}