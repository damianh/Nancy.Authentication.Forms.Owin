namespace Owin
{
    using System;
    using System.Security.Claims;

    public interface IUserManager
    {
        bool Authenticate(string userName, string password, out Guid identifier);

        ClaimsPrincipal GetClaimsPrincial(Guid identifier);
    }
}