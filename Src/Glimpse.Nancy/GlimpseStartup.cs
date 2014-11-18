using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Configuration;
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
            GlimpseRuntime.Initializer.AddInitializationMessage(LoggingLevel.Info, "Added Glimpse.Nancy to Pipelines");

            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                InitializeGlimpse(ctx);

                var requestHandle = GlimpseRuntime.Instance.BeginRequest(GetRequestResponseAdapter(ctx));
                if (requestHandle.RequestHandlingMode != RequestHandlingMode.Unhandled) ctx.SetRequestHandle(requestHandle);

                return null;
            });

            pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (!GlimpseRuntime.IsAvailable) return null;

                var handle = ctx.GetRequestHandle();
                if (handle == null || handle.RequestHandlingMode != RequestHandlingMode.ResourceRequest) return null;

                var queryString = (DynamicDictionary)ctx.Request.Query;
                var resourceName = (string)queryString["n"];

                ctx.Response = new Response();
                GlimpseRuntime.Instance.ExecuteResource(handle, resourceName, new ResourceParameters(queryString.ToStringDictionary()));

                return ctx.Response;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (!GlimpseRuntime.IsAvailable) return;

                var handle = ctx.GetRequestHandle();
                if (handle == null) return;

                GlimpseRuntime.Instance.EndRequest(handle);
            });
        }

        private IRequestResponseAdapter GetRequestResponseAdapter(NancyContext ctx)
        {
            return new NancyRequestResponseAdapter(ctx);
        }

        private void InitializeGlimpse(NancyContext ctx)
        {
            if (GlimpseRuntime.IsAvailable) return;

            lock (InitLock)
            {
                if (GlimpseRuntime.IsAvailable) return;

                GlimpseRuntime.Initializer.AddInitializationMessage(LoggingLevel.Info, "Initializing Glimpse.Nancy Runtime");

                var config = new Configuration(
                    new NancyEndpointConfiguration(ctx),
                    new InMemoryPersistenceStore(new DictionaryDataStoreAdapter(ServerItemsCollection))
                );
                config.Tabs = this.tabs.ToList();
                config.Inspectors = this.inspectors.ToList();

                GlimpseRuntime.Initializer.Initialize(config);
            }
        }
    }
}