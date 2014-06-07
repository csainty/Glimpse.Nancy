using System;
using System.Linq;
using Glimpse.Core.Framework;
using Nancy;
using Nancy.Extensions;

namespace Glimpse.Nancy
{
    internal class NancyRequestMetadata : IRequestMetadata
    {
        private readonly NancyContext context;

        public NancyRequestMetadata(NancyContext context)
        {
            this.context = context;
        }

        public string ClientId
        {
            get { return this.context.Request.UserHostAddress; }
        }

        public string GetCookie(string name)
        {
            if (!this.context.Request.Cookies.ContainsKey(name)) return "";

            return this.context.Request.Cookies[name];
        }

        public string GetHttpHeader(string name)
        {
            // TODO: Should this really be a comma, must be a standard out there
            return String.Join(",", this.context.Request.Headers[name].ToArray());
        }

        public string IpAddress
        {
            get { return this.context.Request.UserHostAddress; }
        }

        public string RequestHttpMethod
        {
            get { return this.context.Request.Method; }
        }

        public bool RequestIsAjax
        {
            get { return this.context.Request.IsAjaxRequest(); }
        }

        public string RequestUri
        {
            get { return this.context.Request.Url.ToString(); }
        }

        public string ResponseContentType
        {
            get { return this.context.Response.ContentType; }
        }

        public int ResponseStatusCode
        {
            get { return (int)this.context.Response.StatusCode; }
        }

        Uri IRequestMetadata.RequestUri
        {
            get { return this.context.Request.Url; }
        }
    }
}