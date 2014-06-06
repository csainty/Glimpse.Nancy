using Glimpse.Nancy.Inspectors;
using Nancy.Bootstrapper;
using Nancy.ViewEngines;

namespace Glimpse.Nancy
{
    public static class InternalConfiguration
    {
        public static void Overrides(NancyInternalConfiguration config)
        {
            if (config.ViewFactory == typeof(DefaultViewFactory)) config.ViewFactory = typeof(GlimpseViewFactory);
            if (config.ViewResolver == typeof(DefaultViewResolver)) config.ViewResolver = typeof(GlimpseViewResolver);
            if (config.ViewLocator == typeof(DefaultViewLocator)) config.ViewLocator = typeof(GlimpseViewLocator);
        }
    }
}