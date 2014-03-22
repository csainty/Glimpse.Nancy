using System;
using Dapper;
using Nancy;

namespace Glimpse.Nancy.TestSite
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IConnectionManager connectionManager)
        {
            Get["/"] = _ =>
            {
                Context.Trace.TraceLog.WriteLog(sb => sb.AppendLine("User requested home page"));

                using (var conn = connectionManager.GetConnection())
                {
                    conn.Execute("INSERT INTO HitLog (IpAddress) VALUES (@ip)", new { ip = "127.0.0.1" });
                }

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

            Get["/hits"] = _ =>
            {
                using (var conn = connectionManager.GetConnection())
                {
                    var hits = conn.Query<string>("SELECT IpAddress FROM HitLog");

                    return Negotiate
                        .WithModel(hits)
                        .WithAllowedMediaRange("text/json");
                }
            };
        }
    }
}