using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy
{
    public class GlimpseStartup : IApplicationStartup
    {
        private readonly IEnumerable<ITab> tabs;
        private readonly IEnumerable<IDisplay> displays;

        public GlimpseStartup(IEnumerable<ITab> tabs, IEnumerable<IDisplay> displays)
        {
            this.tabs = tabs;
            this.displays = displays;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                InitializeGlimpse(ctx);

                GlimpseRuntime.Instance.BeginRequest(GetFrameworkProvider(ctx));
                return null;
            });

            pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx =>
            {
                // TODO: Read this url from the web.config
                if (!String.Equals(ctx.Request.Path, "/glimpse.axd", StringComparison.InvariantCultureIgnoreCase)) return null;
                if (!GlimpseRuntime.IsInitialized) return HttpStatusCode.NotFound;

                var queryString = (DynamicDictionary)ctx.Request.Query;
                string resourceName = queryString["n"];

                ctx.Response = new Response();
                if (string.IsNullOrEmpty(resourceName))
                {
                    GlimpseRuntime.Instance.ExecuteDefaultResource(GetFrameworkProvider(ctx));
                }
                else
                {
                    GlimpseRuntime.Instance.ExecuteResource(GetFrameworkProvider(ctx), resourceName, new ResourceParameters(BuildQueryStringDictionary(queryString)));
                }
                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (!GlimpseRuntime.IsInitialized) return;

                GlimpseRuntime.Instance.EndRequest(GetFrameworkProvider(ctx));
            });
        }

        private IFrameworkProvider GetFrameworkProvider(NancyContext ctx)
        {
            return new NancyFrameworkProvider(ctx, GlimpseRuntime.Instance.Configuration.Logger);
        }

        private void InitializeGlimpse(NancyContext ctx)
        {
            if (GlimpseRuntime.IsInitialized)
            {
                return;
            }

            var config = new GlimpseConfiguration(
                new NancyEndpointConfiguration(ctx),
                new ApplicationPersistenceStore(new DictionaryDataStore(ctx.Items))
            );
            config.Displays = this.displays.ToList();
            config.Tabs = this.tabs.ToList();
            GlimpseRuntime.Initialize(config);
        }

        private static IDictionary<string, string> BuildQueryStringDictionary(DynamicDictionary queryString)
        {
            var d = new Dictionary<string, string>();
            foreach (var key in queryString)
            {
                d.Add(key, queryString[key]);
            }
            return d;
        }
    }
}