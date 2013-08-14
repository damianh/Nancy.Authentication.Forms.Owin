namespace Owin
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;

    public class UserManager : IUserMapper, IUserManager
    {
        private readonly Dictionary<string, User> _usersByUserName = new Dictionary<string, User>();
        private readonly Dictionary<Guid, User> _usersById = new Dictionary<Guid, User>();

        public UserManager()
        {
            var user = new User(Guid.Parse("3718BAB5-F019-43DE-97CE-336F21C86AFB"), "damian", "password");
            _usersById.Add(user.Id, user);
            _usersByUserName.Add(user.UserName, user);
        }


        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            return _usersById[identifier];
        }

        public bool Authenticate(string userName, string password, out Guid identifier)
        {
            if (!_usersByUserName.ContainsKey(userName) || _usersByUserName[userName].Password != password)
            {
                identifier = Guid.Empty;
                return false;
            }
            identifier = _usersByUserName[userName].Id;
            return true;
        }

        public ClaimsPrincipal GetClaimsPrincial(Guid identifier)
        {
            var user = _usersById[identifier];
            return new ClaimsPrincipal(new GenericIdentity(user.UserName));
        }

        private class User : IUserIdentity
        {
            private readonly Guid _id;
            private readonly string _userName;
            private readonly string _password;

            public User(Guid id, string userName, string password)
            {
                _id = id;
                _userName = userName;
                _password = password;
            }

            public Guid Id { get { return _id; } }

            public string Password { get { return _password; } }

            public string UserName
            {
                get { return _userName; }
            }

            public IEnumerable<string> Claims { get; set; }
        }
    }
}