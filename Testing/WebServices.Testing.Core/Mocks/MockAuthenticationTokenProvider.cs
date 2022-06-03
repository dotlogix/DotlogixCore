using System;
using System.Collections.Generic;
using DotLogix.WebServices.Adapters.Http;

namespace DotLogix.WebServices.Testing.Mocks
{
    public class MockAuthenticationTokenProvider : IAuthenticationTokenProvider
    {
        private Func<string> _getDefaultTokenFunc;
        private readonly Dictionary<string, Func<string>> _getDomainTokenFuncMap = new Dictionary<string, Func<string>>(StringComparer.OrdinalIgnoreCase);

        public void UseDefaultToken(string token)
        {
            _getDefaultTokenFunc = () => token;
        }

        public void UseDefaultToken(Func<string> token)
        {
            _getDefaultTokenFunc = token;
        }

        public void UseToken(string domain, string token)
        {
            _getDomainTokenFuncMap[domain] = () => token;
        }

        public void UseToken(string domain, Func<string> token)
        {
            _getDomainTokenFuncMap[domain] = token;
        }

        public void Reset(string domain)
        {
            _getDomainTokenFuncMap.Remove(domain);
        }

        public void Reset()
        {
            _getDefaultTokenFunc = null;
            _getDomainTokenFuncMap.Clear();
        }

        public void ResetDefault()
        {
            _getDefaultTokenFunc = null;
        }


        public string GetAuthenticationToken(Uri url)
        {
            if (_getDomainTokenFuncMap.TryGetValue(url.Host, out var func) == false)
            {
                func = _getDefaultTokenFunc;
            }
            return func?.Invoke();
        }
    }
}