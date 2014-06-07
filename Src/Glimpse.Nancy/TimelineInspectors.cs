using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Nancy.Wrappers;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy
{
    public static class TimelineInspectors
    {
        public static void Enable(NancyInternalConfiguration config)
        {
            config.ViewFactory = typeof(GlimpseViewFactory<>).MakeGenericType(config.ViewFactory);
            config.ViewResolver = typeof(GlimpseViewResolver<>).MakeGenericType(config.ViewResolver);
            config.ViewLocator = typeof(GlimpseViewLocator<>).MakeGenericType(config.ViewLocator);
            config.RouteResolver = typeof(GlimpseRouteResolver<>).MakeGenericType(config.RouteResolver);
            config.Binder = typeof(GlimpseBinder<>).MakeGenericType(config.Binder);
        }

        public static IEnumerable<Type> InspectViewEngines(IEnumerable<Type> viewEngines)
        {
            var wrapperType = typeof(GlimpseViewEngine<>);
            return viewEngines
                .Except(new[] { wrapperType })
                .Select(x => wrapperType.MakeGenericType(x));
        }
    }
}