using Glimpse.Core.Framework;
using Nancy;

namespace Glimpse.Nancy
{
    internal static class NancyContextExtensions
    {
        public static GlimpseRequestContextHandle GetRequestHandle(this NancyContext ctx)
        {
            return (GlimpseRequestContextHandle)ctx.Items["GlimpseRequestContextHandle"];
        }

        public static void SetRequestHandle(this NancyContext ctx, GlimpseRequestContextHandle handle)
        {
            ctx.Items["GlimpseRequestContextHandle"] = handle;
        }
    }
}