namespace Nancy.Authentication.Forms.Owin
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IClaimsPrincipalLookup
    {
        Task<ClaimsPrincipal> GetClaimsPrincial(Guid identifier);
    }
}