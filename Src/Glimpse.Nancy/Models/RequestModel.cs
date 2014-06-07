using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nancy;
using Nancy.Cookies;
using Nancy.Extensions;
using Nancy.Routing;

namespace Glimpse.Nancy.Models
{
    internal class RequestModel
    {
        private readonly NancyContext context;
        private readonly IRootPathProvider rootPathProvider;
        private readonly ResolveResult routeResolution;

        public RequestModel(NancyContext context, IRootPathProvider rootPathProvider, IRouteResolver routeResolver)
        {
            this.context = context;
            this.rootPathProvider = rootPathProvider;
            this.routeResolution = routeResolver.Resolve(context);

            Cookies = this.context.Request.Headers.Cookie.Select(x => new CookieModel(x)).ToArray();
            QueryString = ((DynamicDictionary)this.context.Request.Query).Serialize();
            Parameters = this.routeResolution.Parameters.Serialize();
        }

        //// TODO: Add Form
        //// TODO: Add InputStream

        public CultureInfo CurrentUiCulture { get { return this.context.Culture; } }

        public string ApplicationPath { get { return this.context.ToFullPath("~/"); } }

        public string RequestPath { get { return this.context.Request.Path; } }

        public string PhysicalApplicationPath { get { return this.rootPathProvider.GetRootPath(); } }

        public Uri Url { get { return this.context.Request.Url; } }

        public Uri UrlReferrer { get { return String.IsNullOrEmpty(this.context.Request.Headers.Referrer) ? null : new Uri(this.context.Request.Headers.Referrer); } }

        public string UserAgent { get { return context.Request.Headers.UserAgent; } }

        public string UserHostAddress { get { return context.Request.UserHostAddress; } }

        public string UserHostName { get { return context.Request.Headers.Host; } }

        public IEnumerable<CookieModel> Cookies { get; private set; }

        public IEnumerable<KeyValuePair<string, string>> QueryString { get; private set; }

        public IEnumerable<KeyValuePair<string, string>> Parameters { get; private set; }

        public class CookieModel
        {
            private readonly INancyCookie cookie;

            public CookieModel(INancyCookie cookie)
            {
                this.cookie = cookie;
            }

            public string Name { get { return this.cookie.Name; } }

            public string Path { get { return this.cookie.Path; } }

            public bool IsSecure { get { return this.cookie.Secure; } }

            public string Value { get { return this.cookie.Value; } }

            public bool HttpOnly { get { return this.cookie.HttpOnly; } }
        }
    }
}