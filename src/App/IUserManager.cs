namespace App
{
    using System;

    public interface IUserManager
    {
        bool Authenticate(string userName, string password, out Guid identifier);
    }
}