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

            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                var runtime = GetRuntime(ctx);
                if (runtime.IsInitialized || runtime.Initialize())
                {

                    if (ctx.Request.Path.ToLower() != "/glimpse.axd") return null;
                    if (runtime == null) return HttpStatusCode.NotFound;

                    var queryString = (DynamicDictionary)ctx.Request.Query;
                    string resourceName = queryString["n"];

                    if (string.IsNullOrEmpty(resourceName))
                    {
                        runtime.ExecuteDefaultResource();
                    }
                    else
                    {
                        runtime.ExecuteResource(resourceName, new ResourceParameters(BuildQueryStringDictionary(queryString)));
                    }
                    return "Well what do we do now?";
                }
                return "Why no init?";
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
            var runtime = factory.InstantiateRuntime();
            context.Items[RuntimeKey] = runtime;
            return runtime;
        }
    }
}