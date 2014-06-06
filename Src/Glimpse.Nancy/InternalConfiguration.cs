using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Nancy.Inspectors;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy
{
    public static class InternalConfiguration
    {
        public static void Overrides(NancyInternalConfiguration config)
        {
            config.ViewFactory = typeof(GlimpseViewFactory<>).MakeGenericType(config.ViewFactory);
            config.ViewResolver = typeof(GlimpseViewResolver<>).MakeGenericType(config.ViewResolver);
            config.ViewLocator = typeof(GlimpseViewLocator<>).MakeGenericType(config.ViewLocator);
        }
    }
}