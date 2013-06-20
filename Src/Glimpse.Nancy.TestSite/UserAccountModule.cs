using Nancy;

namespace Glimpse.Nancy.TestSite
{
    public class UserAccountModule : NancyModule
    {
        public UserAccountModule()
            : base("/users")
        {
            Get["/signin"] = _ => 404;
            Post["/signin"] = _ => 404;
            Put["/{userName}/{password}/wtf"] = _ => 404;
            Get["/admin", ctx => false] = _ => 404;
        }
    }
}