// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestResult.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.State;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebRequestResult {
        public bool Succeed => (ReturnValue != null) || (Exception == null);
        public bool Handled { get; private set; }
        public IAsyncHttpContext HttpContext { get; }
        public HttpStatusCode CustomStatusCode { get; set; }
        public object ReturnValue { get; private set; }
        public Type ReturnType { get; set; }
        public Exception Exception { get; private set; }

        public WebRequestResult(IAsyncHttpContext httpContext) {
            HttpContext = httpContext;
        }

        public void SetResult(object result) {
            if(TrySetResult(result) == false)
                throw new InvalidOperationException("Can not set result on a request which is already handled");
        }

        public void SetException(Exception exception) {
            if(TrySetException(exception) == false)
                throw new InvalidOperationException("Can not set exception on a request which is already handled");
        }

        public bool TrySetResult(object result) {
            if(Handled)
                return false;

            Handled = true;
            ReturnValue = result;
            if(ReturnType == null || result != null && result.GetType().IsAssignableTo(ReturnType) == false)
                ReturnType = result.GetType();
            return true;
        }

        public bool TrySetException(Exception exception) {
            if(Handled)
                return false;

            Handled = true;
            Exception = exception;
            return true;
        }
    }
}
