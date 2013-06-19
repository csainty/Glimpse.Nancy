using System;
using System.Collections.Generic;
using System.Globalization;
using Nancy;
using Nancy.Cookies;
using Nancy.Helpers;

namespace Glimpse.Nancy.Models
{
    public class RequestModel
    {
        public RequestModel(NancyContext context)
        {
            var request = context.Request;

            CurrentUiCulture = context.Culture;
            //ApplicationPath = rootPathProvider.GetRootPath();
            Path = request.Path;
            //PhysicalApplicationPath = rootPathProvider.GetRootPath();
            Url = request.Url;
            UrlReferrer = String.IsNullOrEmpty(request.Headers.Referrer) ? null : new Uri(request.Headers.Referrer);
            UserAgent = request.Headers.UserAgent;
            UserHostAddress = request.UserHostAddress;
            UserHostName = request.Headers.Host;

            Cookies = GetCookies(request.Headers.Cookie);
            QueryString = GetQueryString(request.Query);
        }

        //// TODO: Add Form
        //// TODO: Add InputStream

        public CultureInfo CurrentUiCulture { get; private set; }

        public string ApplicationPath { get; private set; }

        public string Path { get; private set; }

        public string PhysicalApplicationPath { get; private set; }

        public Uri Url { get; private set; }

        public Uri UrlReferrer { get; private set; }

        public string UserAgent { get; private set; }

        public string UserHostAddress { get; private set; }

        public string UserHostName { get; private set; }

        public IEnumerable<Cookie> Cookies { get; private set; }

        public IEnumerable<QueryStringParameter> QueryString { get; private set; }

        private IEnumerable<Cookie> GetCookies(IEnumerable<INancyCookie> cookies)
        {
            var result = new List<Cookie>();

            foreach (var cookie in cookies)
            {
                result.Add(new Cookie
                {
                    Name = cookie.Name,
                    Path = cookie.Path,
                    IsSecure = cookie.Secure,
                    Value = HttpUtility.UrlDecode(cookie.Value)
                });
            }

            return result;
        }

        private IEnumerable<QueryStringParameter> GetQueryString(DynamicDictionary queryString)
        {
            foreach (var key in queryString)
            {
                yield return new QueryStringParameter { Key = key, Value = queryString[key] };
            }
        }

        public class QueryStringParameter
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        public class Cookie
        {
            public string Name { get; set; }

            public string Path { get; set; }

            public bool IsSecure { get; set; }

            public string Value { get; set; }
        }
    }
}