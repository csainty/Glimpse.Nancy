using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;

namespace Glimpse.Nancy
{
    public static class GlimpseRequestContextExtensions
    {
        public static void StartTimer(this IGlimpseRequestContext request, string timerName)
        {
            request.RequestStore.Set(timerName, request.CurrentExecutionTimer.Start());
        }

        public static void StopTimer(this IGlimpseRequestContext request, string timerName, string category, string message)
        {
            var timer = request.RequestStore.Get(timerName) as TimeSpan?;
            if (!timer.HasValue) return;
            var result = request.CurrentExecutionTimer.Stop(timer.Value);
            PublishToTimeline(result, category, message);
        }

        public static T TimeAction<T>(this IGlimpseRequestContext request, string category, string message, Func<T> action)
        {
            var timerName = Guid.NewGuid().ToString();
            request.StartTimer(timerName);
            var result = action();
            request.StopTimer(timerName, category, message);
            return result;
        }

        private static void PublishToTimeline(TimerResult result, string category, string message)
        {
            GlimpseRuntime.Instance.Configuration.MessageBroker.Publish(new PipelineMessage { Id = Guid.NewGuid() }
                .AsTimedMessage(result)
                .AsTimelineMessage(message, new TimelineCategoryItem(category, "#000", "#fff"))
            );
        }

        public class PipelineMessage : ITimedMessage, ITimelineMessage
        {
            public TimeSpan Duration { get; set; }

            public TimeSpan Offset { get; set; }

            public DateTime StartTime { get; set; }

            public Guid Id { get; set; }

            public TimelineCategoryItem EventCategory { get; set; }

            public string EventName { get; set; }

            public string EventSubText { get; set; }
        }
    }
}