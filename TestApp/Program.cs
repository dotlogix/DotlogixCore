// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Program.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Attributes.Events;
using DotLogix.Core.Rest.Services.Attributes.Http;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Base;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace TestApp {
    internal class Program {
        private static void Main(string[] args) {
            //var server = new WebServiceHost("http://127.0.0.1:1337/");
            //server.RegisterService<TestService>();

            //server.Start();
            //while(Console.ReadLine() != "stop") {
            //    server.TriggerServerEventAsync("TriggerAdd");
            //}

            //server.Stop();

            var rout = new PatternWebServiceRoute(0, "<<test:g>>/<<id:n>>", HttpMethods.Get, new Auth(), 0);
            Console.Read();
        }
    }

    public class TestService : WebServiceBase {
        public TestService() : base("calc/", "calc") { }

        [WebServiceEvent("TriggerAdd")]
        public event EventHandler TriggerAdd;

        [HttpGet]
        [Auth]
        [DynamicRoute("add")]
        public int Add(int a, int b, int c = 0) {
            return a + b + c;
        }
    }

    public class Auth : PreProcessorAttribute, IWebRequestProcessor {
        public int Priority { get; }
        public bool IgnoreHandled { get; }

        public Task ProcessAsync(WebRequestResult webRequestResult) {
            if((webRequestResult.Context.Request.QueryParameters.TryGetParameterValue("auth", out var obj) == false) || (Equals(obj, "lol") == false))
                webRequestResult.SetException(new RestException(HttpStatusCodes.ClientError.Unauthorized, "Fuck you"));
            return Task.CompletedTask;
        }

        public override IWebRequestProcessor CreateProcessor() {
            return this;
        }
    }
}
