// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpStatusCodes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Rest.Http {
    public static class HttpStatusCodes {
        //1xx – Information
        public static class Information {
            public static readonly HttpStatusCode Continue = new HttpStatusCode(100, "Continue");
            public static readonly HttpStatusCode SwitchingProtocols = new HttpStatusCode(101, "Switching Protocols");
            public static readonly HttpStatusCode Processing = new HttpStatusCode(102, "Processing");
        }

        //2xx – Success
        public static class Success {
            public static readonly HttpStatusCode Ok = new HttpStatusCode(200, "OK");
            public static readonly HttpStatusCode Created = new HttpStatusCode(201, "Created");
            public static readonly HttpStatusCode Accepted = new HttpStatusCode(202, "Accepted");
            public static readonly HttpStatusCode NonAuthoritativeInformation = new HttpStatusCode(203, "Non-Authoritative Information");
            public static readonly HttpStatusCode NoContent = new HttpStatusCode(204, "No Content");
            public static readonly HttpStatusCode ResetContent = new HttpStatusCode(205, "Reset Content");
            public static readonly HttpStatusCode PartialContent = new HttpStatusCode(206, "Partial Content");
            public static readonly HttpStatusCode MultiStatus = new HttpStatusCode(207, "Multi-Status");
            public static readonly HttpStatusCode AlreadyReported = new HttpStatusCode(208, "Already Reported");
            public static readonly HttpStatusCode ImUsed = new HttpStatusCode(226, "IM Used");
        }

        //3xx – Redirect
        public static class Redirect {
            public static readonly HttpStatusCode MultipleChoices = new HttpStatusCode(300, "Multiple Choices");
            public static readonly HttpStatusCode MovedPermanently = new HttpStatusCode(301, "Moved Permanently");
            public static readonly HttpStatusCode FoundMovedTemporarily = new HttpStatusCode(302, "Found (Moved Temporarily)");
            public static readonly HttpStatusCode SeeOther = new HttpStatusCode(303, "See Other");
            public static readonly HttpStatusCode NotModified = new HttpStatusCode(304, "Not Modified");
            public static readonly HttpStatusCode UseProxy = new HttpStatusCode(305, "Use Proxy");
            public static readonly HttpStatusCode TemporaryRedirect = new HttpStatusCode(307, "Temporary Redirect");
            public static readonly HttpStatusCode PermanentRedirect = new HttpStatusCode(308, "Permanent Redirect");
        }

        //4xx – Client-Error
        public static class ClientError {
            public static readonly HttpStatusCode BadRequest = new HttpStatusCode(400, "Bad Request");
            public static readonly HttpStatusCode Unauthorized = new HttpStatusCode(401, "Unauthorized");
            public static readonly HttpStatusCode PaymentRequired = new HttpStatusCode(402, "Payment Required");
            public static readonly HttpStatusCode Forbidden = new HttpStatusCode(403, "Forbidden");
            public static readonly HttpStatusCode NotFound = new HttpStatusCode(404, "Not Found");
            public static readonly HttpStatusCode MethodNotAllowed = new HttpStatusCode(405, "Method Not Allowed");
            public static readonly HttpStatusCode NotAcceptable = new HttpStatusCode(406, "Not Acceptable");
            public static readonly HttpStatusCode ProxyAuthenticationRequired = new HttpStatusCode(407, "Proxy Authentication Required");
            public static readonly HttpStatusCode RequestTime = new HttpStatusCode(408, "Request Time-out");
            public static readonly HttpStatusCode Conflict = new HttpStatusCode(409, "Conflict");
            public static readonly HttpStatusCode Gone = new HttpStatusCode(410, "Gone");
            public static readonly HttpStatusCode LengthRequired = new HttpStatusCode(411, "Length Required");
            public static readonly HttpStatusCode PreconditionFailed = new HttpStatusCode(412, "Precondition Failed");
            public static readonly HttpStatusCode RequestEntityTooLarge = new HttpStatusCode(413, "Request Entity Too Large");
            public static readonly HttpStatusCode RequestUrlTooLong = new HttpStatusCode(414, "Request-URL Too Long");
            public static readonly HttpStatusCode UnsupportedMediaType = new HttpStatusCode(415, "Unsupported Media Type");
            public static readonly HttpStatusCode RequestedRangeNotSatisfiable = new HttpStatusCode(416, "Requested range not satisfiable");
            public static readonly HttpStatusCode ExpectationFailed = new HttpStatusCode(417, "Expectation Failed");
            public static readonly HttpStatusCode ImATeapot = new HttpStatusCode(418, "I'm a teapot");
            public static readonly HttpStatusCode PolicyNotFulfilled = new HttpStatusCode(420, "Policy Not Fulfilled");
            public static readonly HttpStatusCode MisdirectedRequest = new HttpStatusCode(421, "Misdirected Request");
            public static readonly HttpStatusCode UnprocessableEntity = new HttpStatusCode(422, "Unprocessable Entity");
            public static readonly HttpStatusCode Locked = new HttpStatusCode(423, "Locked");
            public static readonly HttpStatusCode FailedDependency = new HttpStatusCode(424, "Failed Dependency");
            public static readonly HttpStatusCode UnorderedCollection = new HttpStatusCode(425, "Unordered Collection");
            public static readonly HttpStatusCode UpgradeRequired = new HttpStatusCode(426, "Upgrade Required");
            public static readonly HttpStatusCode PreconditionRequired = new HttpStatusCode(428, "Precondition Required");
            public static readonly HttpStatusCode TooManyRequests = new HttpStatusCode(429, "Too Many Requests");
            public static readonly HttpStatusCode RequestHeaderFieldsTooLarge = new HttpStatusCode(431, "Request Header Fields Too Large");
            public static readonly HttpStatusCode UnavailableForLegalReasons = new HttpStatusCode(451, "Unavailable For Legal Reasons");
        }

        //Other
        public static class Other {
            public static readonly HttpStatusCode NoResponse = new HttpStatusCode(444, "No Response");
            public static readonly HttpStatusCode TheRequestShouldBeRetriedAfterDoingTheAppropriateAction = new HttpStatusCode(449, "The request should be retried after doing the appropriate action");
            public static readonly HttpStatusCode ClientClosedRequest = new HttpStatusCode(499, "Client Closed Request");
        }

        //5xx – Server-Error
        public static class ServerError {
            public static readonly HttpStatusCode InternalServerError = new HttpStatusCode(500, "Internal Server Error");
            public static readonly HttpStatusCode NotImplemented = new HttpStatusCode(501, "Not Implemented");
            public static readonly HttpStatusCode BadGateway = new HttpStatusCode(502, "Bad Gateway");
            public static readonly HttpStatusCode ServiceUnavailable = new HttpStatusCode(503, "Service Unavailable");
            public static readonly HttpStatusCode GatewayTimeOut = new HttpStatusCode(504, "Gateway Time-out");
            public static readonly HttpStatusCode HttpVersionNotSupported = new HttpStatusCode(505, "HTTP Version not supported");
            public static readonly HttpStatusCode VariantAlsoNegotiates = new HttpStatusCode(506, "Variant Also Negotiates");
            public static readonly HttpStatusCode InsufficientStorage = new HttpStatusCode(507, "Insufficient Storage");
            public static readonly HttpStatusCode LoopDetected = new HttpStatusCode(508, "Loop Detected");
            public static readonly HttpStatusCode BandwidthLimitExceeded = new HttpStatusCode(509, "Bandwidth Limit Exceeded");
            public static readonly HttpStatusCode NotExtended = new HttpStatusCode(510, "Not Extended");
            public static readonly HttpStatusCode NetworkAuthenticationRequired = new HttpStatusCode(511, "Network Authentication Required");
        }

        public static HttpStatusCodeGroup GetGroup(int code) {
            return (HttpStatusCodeGroup)((code / 100) % 10);
        }
    }
}
