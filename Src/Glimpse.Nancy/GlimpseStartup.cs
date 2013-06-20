using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Routing;

namespace Glimpse.Nancy
{
    public class GlimpseStartup : IApplicationStartup
    {
        private const string RuntimeKey = "_glimpse_runtime";

        private readonly IEnumerable<ITab> tabs;

        public GlimpseStartup(IEnumerable<ITab> tabs)
        {
            this.tabs = tabs;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                var runtime = GetRuntime(ctx);
                if (runtime.IsInitialized)
                {
                    runtime.BeginRequest();
                }
                return null;
            });

            pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx =>
            {
                // TODO: Read this url from the web.config
                if (ctx.Request.Path.ToLower() != "/glimpse.axd") return null;

                var runtime = GetRuntime(ctx);
                if (runtime == null) return HttpStatusCode.NotFound;

                if (runtime.IsInitialized)
                {
                    var queryString = (DynamicDictionary)ctx.Request.Query;
                    string resourceName = queryString["n"];

                    ctx.Response = new Response();
                    if (string.IsNullOrEmpty(resourceName))
                    {
                        runtime.ExecuteDefaultResource();
                    }
                    else
                    {
                        runtime.ExecuteResource(resourceName, new ResourceParameters(BuildQueryStringDictionary(queryString)));
                    }
                }
                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                var runtime = GetRuntime(ctx);
                if (runtime.IsInitialized)
                {
                    runtime.EndRequest();
                }
            });
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

        private IGlimpseRuntime GetRuntime(NancyContext context)
        {
            if (context.Items.ContainsKey(RuntimeKey))
            {
                return context.Items[RuntimeKey] as IGlimpseRuntime;
            }

            var serviceLocator = new NancyServiceLocator(context);
            serviceLocator.Tabs = this.tabs;

            var factory = new Factory(serviceLocator);
            serviceLocator.Logger = factory.InstantiateLogger();
            var runtime = factory.InstantiateRuntime();
            runtime.Initialize();
            context.Items[RuntimeKey] = runtime;
            return runtime;
        }
    }
}