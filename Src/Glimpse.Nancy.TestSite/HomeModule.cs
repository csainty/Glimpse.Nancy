using System;
using Nancy;

namespace Glimpse.Nancy.TestSite
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                Context.Trace.TraceLog.WriteLog(sb => sb.AppendLine("User requested home page"));

                return Negotiate
                    .WithModel(new { Foo = "Bar", Now = DateTime.Now })
                    .WithAllowedMediaRange("text/json")
                    .WithAllowedMediaRange("text/html")
                    .WithAllowedMediaRange("text/xml")
                    .WithView("Index");
            };

            Get["/greet/{name}"] = _ =>
            {
                return View["Greet", new { Name = _.name }];
            };
        }
    }
}