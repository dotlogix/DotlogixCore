#region using
using System.Linq;
using System.Net;
#endregion

namespace DotLogix.WebServices.AspCore.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IndicatesInformation(this HttpStatusCode statusCode)
        {
            return statusCode.MatchesGroup(1); // 1xx Information
        }

        public static bool IndicatesSuccess(this HttpStatusCode statusCode)
        {
            return statusCode.MatchesGroup(2); // 2xx Success
        }

        public static bool IndicatesRedirect(this HttpStatusCode statusCode)
        {
            return statusCode.MatchesGroup(3); // 3xx Redirect
        }

        public static bool IndicatesClientError(this HttpStatusCode statusCode)
        {
            return statusCode.MatchesGroup(4); // 4xx Client Error
        }

        public static bool IndicatesServerError(this HttpStatusCode statusCode)
        {
            return statusCode.MatchesGroup(5); // 5xx Server Error
        }

        public static bool IndicatesError(this HttpStatusCode statusCode)
        {
            return statusCode.IndicatesClientError() || statusCode.IndicatesServerError(); // 4xx | 5xx Any Error
        }

        public static bool EqualsAny(this HttpStatusCode statusCode, params HttpStatusCode[] statusCodes)
        {
            return statusCodes.Contains(statusCode);
        }

        public static bool InRange(this HttpStatusCode statusCode, int min, int max)
        {
            return (int) statusCode >= min && (int) statusCode <= max;
        }

        public static bool MatchesGroup(this HttpStatusCode statusCode, int statusCodeGroup)
        {
            return ((int) statusCode / 100) == statusCodeGroup;
        }
    }
}