// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  ErrorHandlingApiClient.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 15.04.2022 16:24
// LastEdited:  15.04.2022 16:24
// ==================================================

using DotLogix.WebServices.Adapters.Endpoints;
using DotLogix.WebServices.Adapters.Http;

namespace DotLogix.WebServices.Adapters.Client;

public class ErrorHandlingApiClient : ApiClient
{
    public ErrorHandlingApiClient(IHttpProvider httpProvider, IWebServiceEndpoint endpoint, string relativeUri)
        : base(new ErrorHandlingHttpProvider(httpProvider), endpoint, relativeUri)
    {
    }
}
