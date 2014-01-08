using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using Nancy;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Replacements
{
    public class GlimpseViewEngine : IViewEngine
    {
        private readonly IEnumerable<IViewEngine> viewEngines;

        public GlimpseViewEngine(IEnumerable<IViewEngine> viewEngines)
        {
            this.viewEngines = viewEngines;
        }

        public IEnumerable<string> Extensions
        {
            get { return this.viewEngines.Where(x => x != this).SelectMany(x => x.Extensions); }
        }

        public void Initialize(ViewEngineStartupContext viewEngineStartupContext)
        {
        }

        public Response RenderView(ViewLocationResult viewLocationResult, dynamic model, IRenderContext renderContext)
        {
            foreach (var engine in this.viewEngines.Where(x => x != this))
            {
                if (engine.Extensions.Contains(viewLocationResult.Extension))
                {
                    var timer = GlimpseRuntime.Instance.Configuration.TimerStrategy();
                    var result = timer.Time(() => engine.RenderView(viewLocationResult, model, renderContext));
                    GlimpseRuntime.Instance.Configuration.MessageBroker.Publish(new Message { Id = Guid.NewGuid() }
                        .AsTimedMessage(result)
                        .AsTimelineMessage("Render View", new TimelineCategoryItem("Views", "#999", "#bbb"))
                    );
                    return result.Result;
                }
            }
            return null;
        }

        public class Message : ITimedMessage, ITimelineMessage
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