using System.Collections.Generic;
using System.IO;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Nancy.Filters;
using Nancy;

namespace Glimpse.Nancy
{
    public class NancyFrameworkProvider : IFrameworkProvider
    {
        private static IDataStore ServerStore = new DictionaryDataStore(new Dictionary<string, object>());

        private readonly NancyContext context;
        private readonly IDataStore contextDataStore;
        private readonly ILogger logger;

        public NancyFrameworkProvider(NancyContext ctx, ILogger logger)
        {
            this.context = ctx;
            this.logger = logger;
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
            var capturedContent = new MemoryStream();
            this.context.Response.Contents(capturedContent);
            capturedContent.Seek(0, SeekOrigin.Begin);

            // TODO: UTF8?
            this.context.Response.Contents = s =>
            {
                using (var filter = new PreBodyTagFilter(htmlSnippet, s, Encoding.UTF8, this.logger))
                {
                    capturedContent.CopyTo(filter);
                    capturedContent.Dispose();
                }
            };
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
            if (name == "Content-Type") this.context.Response.ContentType = value;
            this.context.Response.Headers[name] = value;
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            this.context.Response.StatusCode = (HttpStatusCode)statusCode;
        }

        public void WriteHttpResponse(string content)
        {
            // TODO: UTF8?
            var bytes = Encoding.UTF8.GetBytes(content);
            this.WriteHttpResponse(bytes);
        }

        public void WriteHttpResponse(byte[] content)
        {
            this.context.Response.Contents = s => { s.Write(content, 0, content.Length); };
        }
    }
}