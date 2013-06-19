using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;

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
            get { throw new NotImplementedException(); }
        }

        public object RuntimeContext
        {
            get { return this.context; }
        }

        public void SetCookie(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            throw new NotImplementedException();
        }

        public void WriteHttpResponse(string content)
        {
            throw new NotImplementedException();
        }

        public void WriteHttpResponse(byte[] content)
        {
            throw new NotImplementedException();
        }
    }
}