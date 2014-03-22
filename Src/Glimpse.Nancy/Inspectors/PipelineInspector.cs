using Nancy.Bootstrapper;

namespace Glimpse.Nancy.Inspectors
{
    public class PipelineInspector : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx =>
            {
                var request = ctx.TryGetRequestContext();
                if (request == null) return null;

                request.StartTimer("MainPipelineTimer");
                return null;
            });

            pipelines.AfterRequest.AddItemToStartOfPipeline(ctx =>
            {
                var request = ctx.TryGetRequestContext();
                if (request == null) return;

                request.StopTimer("MainPipelineTimer", "Pipeline", "Request Processing");
            });
        }
    }
}