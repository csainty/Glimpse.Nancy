using System.Collections.Generic;
using System.Linq;
using global::Nancy.Security;

namespace Glimpse.Nancy.Models
{
    public class AuthenticationModel
    {
        private IUserIdentity userIdentity;

        public AuthenticationModel(IUserIdentity userIdentity)
        {
            this.userIdentity = userIdentity;
        }

        public bool IsAuthenticated { get { return this.userIdentity.IsAuthenticated(); } }

        public string UserName { get { return this.userIdentity == null ? "" : this.userIdentity.UserName; } }

        public IEnumerable<string> Claims { get { return this.userIdentity == null ? Enumerable.Empty<string>() : this.userIdentity.Claims; } }
    }
}