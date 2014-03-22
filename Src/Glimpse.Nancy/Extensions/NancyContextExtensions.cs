using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;

namespace Glimpse.Nancy
{
    internal static class NancyContextExtensions
    {
        public static GlimpseRequestContextHandle GetRequestHandle(this NancyContext ctx)
        {
            object handle;
            if (!ctx.Items.TryGetValue("GlimpseRequestContextHandle", out handle))
            {
                return null;
            }
            return (GlimpseRequestContextHandle)handle;
        }

        public static void SetRequestHandle(this NancyContext ctx, GlimpseRequestContextHandle handle)
        {
            ctx.Items["GlimpseRequestContextHandle"] = handle;
        }

        public static IGlimpseRequestContext TryGetRequestContext(this NancyContext ctx)
        {
            IGlimpseRequestContext request;

            var handle = ctx.GetRequestHandle();
            if (handle == null || handle.RequestHandlingMode == RequestHandlingMode.Unhandled) return null;
            if (!GlimpseRuntime.Instance.TryGetRequestContext(handle.GlimpseRequestId, out request)) return null;
            return request;
        }
    }
}