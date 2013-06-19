using System;
using Nancy;
using Nancy.Responses.Negotiation;

namespace Glimpse.Nancy.TestSite
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                return Negotiate
                    .WithModel(new { Foo = "Bar", Now = DateTime.Now })
                    .WithAllowedMediaRange("text/json")
                    .WithAllowedMediaRange("text/html")
                    .WithAllowedMediaRange("text/xml")
                    .WithView("Index");
            };
        }
    }
}