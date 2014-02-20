using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Extensions;

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

                var requestHandle = GlimpseRuntime.Instance.BeginRequest(GetRequestResponseAdapter(ctx));
                if (requestHandle.RequestHandlingMode != RequestHandlingMode.Unhandled) ctx.SetRequestHandle(requestHandle);

                return null;
            });

            pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (!GlimpseRuntime.IsInitialized) return null;
                var glimpseUrl = ctx.ToFullPath(GlimpseRuntime.Instance.Configuration.EndpointBaseUri);
                if (!String.Equals(ctx.Request.Path, glimpseUrl, StringComparison.InvariantCultureIgnoreCase)) return null;

                var handle = ctx.GetRequestHandle();
                if (handle.RequestHandlingMode == RequestHandlingMode.Unhandled) return null;

                var queryString = (DynamicDictionary)ctx.Request.Query;
                string resourceName = queryString["n"];

                ctx.Response = new Response();
                if (string.IsNullOrEmpty(resourceName))
                {
                    GlimpseRuntime.Instance.ExecuteDefaultResource(handle);
                }
                else
                {
                    GlimpseRuntime.Instance.ExecuteResource(handle, resourceName, new ResourceParameters(queryString.ToDictionary()));
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
    }
}