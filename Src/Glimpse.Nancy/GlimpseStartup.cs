using System.Collections.Generic;
using Glimpse.Core.Framework;
using Nancy;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy
{
    public class GlimpseStartup : IApplicationStartup
    {
        private const string RuntimeKey = "_glimpse_runtime";

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                var runtime = GetRuntime(ctx);
                if (runtime.IsInitialized || runtime.Initialize())
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

                if (runtime.IsInitialized || runtime.Initialize())
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
                if (runtime.IsInitialized || runtime.Initialize())
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

        private static IGlimpseRuntime GetRuntime(NancyContext context)
        {
            if (context.Items.ContainsKey(RuntimeKey))
            {
                return context.Items[RuntimeKey] as IGlimpseRuntime;
            }

            var serviceLocator = new NancyServiceLocator(context);
            var factory = new Factory(serviceLocator);
            serviceLocator.Logger = factory.InstantiateLogger();
            var runtime = factory.InstantiateRuntime();
            context.Items[RuntimeKey] = runtime;
            return runtime;
        }
    }
}