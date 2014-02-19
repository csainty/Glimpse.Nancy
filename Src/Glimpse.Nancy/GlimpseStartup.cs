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

        public GlimpseStartup(IEnumerable<ITab> tabs)
        {
            this.tabs = tabs;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                InitializeGlimpse(ctx);

                ctx.SetRequestHandle(GlimpseRuntime.Instance.BeginRequest(GetRequestResponseAdapter(ctx)));
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
                    GlimpseRuntime.Instance.ExecuteDefaultResource(ctx.GetRequestHandle());
                }
                else
                {
                    GlimpseRuntime.Instance.ExecuteResource(ctx.GetRequestHandle(), resourceName, new ResourceParameters(BuildQueryStringDictionary(queryString)));
                }
                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (!GlimpseRuntime.IsInitialized) return;

                GlimpseRuntime.Instance.EndRequest(ctx.GetRequestHandle());
            });
        }

        private IRequestResponseAdapter GetRequestResponseAdapter(NancyContext ctx)
        {
            return new NancyRequestResponseAdapter(ctx, GlimpseRuntime.Instance.Configuration.Logger);
        }

        private void InitializeGlimpse(NancyContext ctx)
        {
            if (GlimpseRuntime.IsInitialized)
            {
                return;
            }

            var config = new GlimpseConfiguration(
                new NancyEndpointConfiguration(ctx),
                new InMemoryPersistenceStore(new DictionaryDataStore(ctx.Items))
            );
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