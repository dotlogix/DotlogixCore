using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Services {
    public class RedirectResultWriter : PrimitiveResultWriter {
        public static IWebServiceResultWriter Instance { get; } = new RedirectResultWriter();

        private RedirectResultWriter() { }

        /// <inheritdoc />
        public async Task WriteAsync(WebServiceContext context) {
            var response = context.HttpResponse;
            if (response.IsCompleted)
                return;

            var contentResult = context.Result switch {
                IRedirectWebServiceResult redirectResult => redirectResult,
                IWebServiceObjectResult objResult => objResult.ReturnValue.Value as IRedirectWebServiceResult,
                _ => null
            };

            if(contentResult == null) {
                await base.WriteAsync(context);
                return;
            }

            response.HeaderParameters.Add("Location", contentResult.RedirectTo);
            response.StatusCode = contentResult.StatusCode;
            response.ContentType = contentResult.ContentType;
            await response.CompleteAsync();
        }
    }
}