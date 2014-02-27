using System;
using System.Data.SQLite;
using Nancy;
using Dapper;
using System.Data.Common;

namespace Glimpse.Nancy.TestSite
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                Context.Trace.TraceLog.WriteLog(sb => sb.AppendLine("User requested home page"));

                var factory = DbProviderFactories.GetFactory("System.Data.SQLite");

                using (var conn = factory.CreateConnection())
                {
                    conn.ConnectionString = "Data Source=:memory:";
                    conn.Open();
                    conn.Execute("CREATE TABLE Foo (Id INTEGER PRIMARY KEY)");
                    conn.Query("SELECT * FROM Foo");
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
        }
    }
}