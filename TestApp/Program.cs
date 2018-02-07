using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Attributes.Events;
using DotLogix.Core.Rest.Services.Attributes.Http;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Base;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Tracking;
using DotLogix.Core.Tracking.Entries;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args) {
            //var server = new WebServiceHost("http://127.0.0.1:1337/");
            //server.RegisterService<TestService>();

            //server.Start();
            //while(Console.ReadLine() != "stop") {
            //    server.TriggerServerEventAsync("TriggerAdd");
            //}

            //server.Stop();

            var rout = new PatternWebServiceRoute("<<test:g>>/<<id:n>>", HttpMethods.Get, new Auth(), 0);
            Console.Read();


        }
    }

    public class TestService : WebServiceBase {
        [WebServiceEvent("TriggerAdd")]
        public event EventHandler TriggerAdd;

        public TestService() : base("calc/", "calc") { }

        [HttpGet]
        [Auth]
        [DynamicRoute("add")]
        public int Add(int a, int b, int c=0) {
            return a + b + c;
        }
    }

    public class Auth : PreProcessorAttribute, IWebRequestProcessor
    {
        public override IWebRequestProcessor CreateProcessor() {
            return this;
        }

        public int Priority { get; }
        public bool IgnoreHandled { get; }
        public Task ProcessAsync(WebRequestResult webRequestResult) {
            if(webRequestResult.Context.Request.QueryParameters.TryGetParameterValue("auth", out var obj) == false || Equals(obj, "lol") == false) {
                webRequestResult.SetException(new RestException(HttpStatusCodes.Unauthorized, "Fuck you"));
            }
            return Task.CompletedTask;
        }
    }
}
