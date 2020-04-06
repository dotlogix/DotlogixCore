// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceEvent.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Routes;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceEvent : IWebServiceEvent {
        private readonly object _subscriptionLock = new object();
        private readonly HashSet<IWebServiceEventSubscription> _subscriptions = new HashSet<IWebServiceEventSubscription>();

        public WebServiceEvent(string name) {
            Name = name;
        }

        public string Name { get; }

        public void Subscribe(IAsyncHttpContext context, IWebServiceRoute route, AsyncWebRequestRouter router) {
            Subscribe(CreateSubscription(context, route, router));
        }

        public async Task DispatchAsync(object sender, WebServiceEventArgs eventArgs) {
            List<IWebServiceEventSubscription> subscriptions;
            lock(_subscriptionLock) {
                subscriptions = _subscriptions.Where(s => s.CheckPreCondition(sender, eventArgs)).ToList();
                if(subscriptions.Count == _subscriptions.Count)
                    _subscriptions.Clear();
                else
                    _subscriptions.ExceptWith(subscriptions);
            }

            foreach(var subscription in subscriptions) {
                var asyncHttpContext = subscription.Context;
                var response = asyncHttpContext.Response;
                try {
                    await subscription.Router.HandleAsync(asyncHttpContext, subscription.Route);
                    if(response.IsCompleted == false)
                        await response.CompleteAsync();
                } catch(Exception e) {
                    Log.Error(e);
                    await AsyncHttpServer.SendErrorMessageAsync(response, e);
                }
            }
        }

        protected virtual WebServiceEventSubscription CreateSubscription(IAsyncHttpContext context, IWebServiceRoute route, AsyncWebRequestRouter router) {
            return new WebServiceEventSubscription(context, route, router);
        }

        protected void Subscribe(IWebServiceEventSubscription subscription) {
            lock(_subscriptionLock)
                _subscriptions.Add(subscription);
        }
    }
}
