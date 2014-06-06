using System.Collections.Generic;
using Dapper;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;
using Nancy.Security;
using Nancy.TinyIoc;

namespace Glimpse.Nancy.TestSite
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            StaticConfiguration.EnableRequestTracing = true;

            var authConfig = new StatelessAuthenticationConfiguration(ctx =>
            {
                if (!ctx.Request.Query.auth.HasValue)
                {
                    return null;
                }
                return new User();
            });
            StatelessAuthentication.Enable(pipelines, authConfig);

            var connectionFactory = container.Resolve<IConnectionManager>();
            using (var conn = connectionFactory.GetConnection())
            {
                conn.Execute("CREATE TABLE IF NOT EXISTS HitLog (Id INTEGER PRIMARY KEY, IpAddress STRING)");
            }
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"password" }; }
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(Glimpse.Nancy.InternalConfiguration.Overrides);
            }
        }

        private class User : IUserIdentity
        {
            public IEnumerable<string> Claims
            {
                get { return new[] { "Awesome", "Ninja" }; }
            }

            public string UserName
            {
                get { return "Foo"; }
            }
        }
    }
}