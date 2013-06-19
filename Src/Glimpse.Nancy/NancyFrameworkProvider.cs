using System;
using System.Collections.Generic;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;
using Nancy.Extensions;

namespace Glimpse.Nancy
{
    public class NancyFrameworkProvider : IFrameworkProvider
    {
        private static IDataStore ServerStore = new DictionaryDataStore(new Dictionary<string, object>());

        private readonly NancyContext context;
        private readonly IDataStore contextDataStore;

        public NancyFrameworkProvider(NancyContext ctx)
        {
            this.context = ctx;
            this.contextDataStore = new DictionaryDataStore(ctx.Items);
        }

        public IDataStore HttpRequestStore
        {
            get { return this.contextDataStore; }
        }

        public IDataStore HttpServerStore
        {
            get { return ServerStore; }
        }

        public void InjectHttpResponseBody(string htmlSnippet)
        {
            throw new NotImplementedException();
        }

        public IRequestMetadata RequestMetadata
        {
            get { return new NancyRequestMetadata(this.context); }
        }

        public object RuntimeContext
        {
            get { return this.context; }
        }

        public void SetCookie(string name, string value)
        {
            this.context.Response.AddCookie(name, value);
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            this.context.Response.Headers[name] = value;
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            this.context.Response.StatusCode = (HttpStatusCode)statusCode;
        }

        public void WriteHttpResponse(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            this.WriteHttpResponse(bytes);
        }

        public void WriteHttpResponse(byte[] content)
        {
            this.context.Response.Contents = s => { s.Write(content, 0, content.Length); };
        }
    }
}