using System.Collections.Concurrent;
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
        private readonly IEnumerable<IInspector> inspectors;
        private static readonly object InitLock = new object();
        private static readonly ConcurrentDictionary<string, object> ServerItemsCollection = new ConcurrentDictionary<string, object>();

        public GlimpseStartup(IEnumerable<ITab> tabs, IEnumerable<IInspector> inspectors)
        {
            this.tabs = tabs;
            this.inspectors = inspectors;
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

                var handle = ctx.GetRequestHandle();
                if (handle == null || handle.RequestHandlingMode != RequestHandlingMode.ResourceRequest) return null;

                var queryString = (DynamicDictionary)ctx.Request.Query;
                var resourceName = (string)queryString["n"];

                ctx.Response = new Response();
                if (string.IsNullOrEmpty(resourceName))
                {
                    GlimpseRuntime.Instance.ExecuteDefaultResource(handle);
                }
                else
                {
                    GlimpseRuntime.Instance.ExecuteResource(handle, resourceName, new ResourceParameters(queryString.ToDictionary()));
                }
                return ctx.Response;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (!GlimpseRuntime.IsInitialized) return;

                var handle = ctx.GetRequestHandle();
                if (handle == null) return;

                GlimpseRuntime.Instance.EndRequest(handle);
            });
        }

        private IRequestResponseAdapter GetRequestResponseAdapter(NancyContext ctx)
        {
            return new NancyRequestResponseAdapter(ctx, GlimpseRuntime.Instance.Configuration.Logger);
        }

        private void InitializeGlimpse(NancyContext ctx)
        {
            if (GlimpseRuntime.IsInitialized) return;

            lock (InitLock)
            {
                if (GlimpseRuntime.IsInitialized) return;

                var config = new GlimpseConfiguration(
                    new NancyEndpointConfiguration(ctx),
                    new InMemoryPersistenceStore(new DictionaryDataStoreAdapter(ServerItemsCollection))
                );
                config.Tabs = this.tabs.ToList();
                config.Inspectors = this.inspectors.ToList();
                GlimpseRuntime.Initialize(config);
            }
        }
    }
}