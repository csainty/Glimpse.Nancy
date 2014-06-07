using System;
using System.Text;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using Nancy;
using Nancy.Diagnostics;

namespace Glimpse.Nancy.Wrappers
{
    public class GlimpseTraceLog : ITraceLog
    {
        private readonly ITraceLog log;
        private readonly NancyContext context;
        private TimeSpan lastOffset = TimeSpan.Zero;
        private bool hasPublishedExistingMessages = false;

        public GlimpseTraceLog(ITraceLog wrappedLog, NancyContext context)
        {
            this.log = wrappedLog;
            this.context = context;
        }

        public void WriteLog(Action<StringBuilder> logDelegate)
        {
            if (!GlimpseRuntime.IsAvailable)
            {
                log.WriteLog(logDelegate);
                return;
            }

            if (!hasPublishedExistingMessages) PublishExistingLogMessages();

            var request = context.TryGetRequestContext();
            if (request == null) return;

            var now = request.CurrentExecutionTimer.Point();
            Publish(UnwrapMessage(logDelegate), now.Offset, now.Offset - lastOffset);
            lastOffset = now.Offset;

            log.WriteLog(logDelegate);
        }

        private string UnwrapMessage(Action<StringBuilder> logDelegate)
        {
            var builder = new StringBuilder();
            logDelegate(builder);
            return builder.ToString();
        }

        private void Publish(string message, TimeSpan? fromFirst = null, TimeSpan? fromLast = null)
        {
            var broker = GlimpseRuntime.Instance.Configuration.MessageBroker;
            broker.Publish(new TraceMessage
            {
                Message = message,
                Category = "Nancy.TraceLog",
                FromFirst = fromFirst ?? TimeSpan.Zero,
                FromLast = fromLast ?? TimeSpan.Zero
            });
        }

        private void PublishExistingLogMessages()
        {
            hasPublishedExistingMessages = true;
            var traceMessages = this.log.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var msg in traceMessages)
            {
                Publish(msg);
            }
        }
    }
}