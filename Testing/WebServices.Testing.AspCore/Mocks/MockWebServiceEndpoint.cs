using System;
using DotLogix.WebServices.Adapters.Endpoints;

namespace DotLogix.WebServices.Testing.AspCore.Mocks
{
    public class MockWebServiceEndpoint : IWebServiceEndpoint
    {
        private Func<Uri> _getCurrentFunc;
        private string _relativeUri;

        public Uri BaseUri => _getCurrentFunc?.Invoke();
        public Uri Uri => string.IsNullOrEmpty(_relativeUri) ? BaseUri : new Uri(BaseUri, _relativeUri);

        public void UseUri(Uri uri)
        {
            _getCurrentFunc = () => uri;
        }

        public void UseUri(Func<Uri> getUriFunc)
        {
            _getCurrentFunc = getUriFunc;
        }

        public void UseRelativePath(string relativeUri)
        {
            _relativeUri = relativeUri;
        }

        public void Reset()
        {
            _getCurrentFunc = null;
            _relativeUri = null;
        }

        public void ResetPath()
        {
            _relativeUri = null;
        }
    }
}