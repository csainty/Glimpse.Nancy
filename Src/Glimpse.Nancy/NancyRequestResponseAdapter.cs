using System.IO;
using System.Text;
using Glimpse.Core.Framework;
using Nancy;

namespace Glimpse.Nancy
{
    internal class NancyRequestResponseAdapter : IRequestResponseAdapter
    {
        private readonly NancyContext context;
        private LateWrappingStream capturingStream = new LateWrappingStream();
        private Stream glimpseProvidedStream;
        private bool hasSwitchedStream = false;

        public NancyRequestResponseAdapter(NancyContext ctx)
        {
            this.context = ctx;
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
            TrySwitchStream();
            this.context.Response.WithCookie(name, value);
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            TrySwitchStream();
            if (name == "Content-Type") this.context.Response.ContentType = value;
            this.context.Response.Headers[name] = value;
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            TrySwitchStream();
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

        public Stream OutputStream
        {
            get
            {
                return this.capturingStream;
            }
            set
            {
                this.glimpseProvidedStream = value;
            }
        }

        public Encoding ResponseEncoding
        {
            get { return Encoding.UTF8; }
        }

        private void TrySwitchStream()
        {
            if (this.hasSwitchedStream)
            {
                return;
            }

            this.hasSwitchedStream = true;
            var originalContents = this.context.Response.Contents;
            this.context.Response.Contents = nancyStream =>
            {
                this.capturingStream.StreamToWriteTo = nancyStream;
                originalContents(this.glimpseProvidedStream);
                this.glimpseProvidedStream.Flush();
            };
        }
    }
}