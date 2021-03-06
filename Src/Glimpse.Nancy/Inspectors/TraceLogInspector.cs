﻿using Glimpse.Nancy.Wrappers;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy.Inspectors
{
    public class TraceLogInspector : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                if (ctx.Trace == null || ctx.Trace.TraceLog == null) return null;

                ctx.Trace.TraceLog = new GlimpseTraceLog(ctx.Trace.TraceLog, ctx);
                return null;
            });
        }
    }
}