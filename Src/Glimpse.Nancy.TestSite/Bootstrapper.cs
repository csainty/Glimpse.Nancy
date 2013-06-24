using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Glimpse.Nancy.TestSite
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            StaticConfiguration.EnableRequestTracing = true;
        }
    }
}