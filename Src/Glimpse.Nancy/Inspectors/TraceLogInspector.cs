using System;
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
                var traceMessages = ctx.Trace.TraceLog.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (traceMessages.Length > 0)
                {
                    var broker = ctx.GetMessageBroker();
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