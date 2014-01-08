using System;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy.Inspectors
{
    public class TraceLogInspector : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToStartOfPipeline(ctx =>
            {
                if (!GlimpseRuntime.IsInitialized) return;

                var traceMessages = ctx.Trace.TraceLog.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (traceMessages.Length > 0)
                {
                    var broker = GlimpseRuntime.Instance.Configuration.MessageBroker;
                    foreach (var msg in traceMessages)
                    {
                        broker.Publish(new TraceMessage
                        {
                            Message = msg,
                            Category = "Nancy.TraceLog"
                        });
                    }
                }
            });
        }
    }
}