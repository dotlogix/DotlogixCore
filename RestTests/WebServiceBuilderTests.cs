using System;
using System.Linq;
using System.Text.RegularExpressions;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Routing.Matching;
using NUnit.Framework;

namespace RestTests {
    [TestFixture]
    public class WebServiceBuilderTests {
        private static readonly MockWebService _serviceInstance = new MockWebService();
        
        [SetUp]
        public void Setup() {

        }

        [Test]
        public void WebServiceBuilder_Build_NoServiceOrFactory_Throws() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            Assert.That(() => builder.Build(0), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void WebServiceBuilder_Name() {
            var expected = "test";
            
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);
            builder.UseName(expected);

            var webServiceType = builder.Build(0);
            Assert.That(webServiceType.Name, Is.EqualTo(expected));
        }

        [Test]
        public void WebServiceBuilder_HasDefaultName() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);

            var webServiceType = builder.Build(0);
            Assert.That(webServiceType.Name, Does.StartWith(nameof(MockWebService)));
        }

        [Test]
        public void WebServiceBuilder_RoutePrefix() {
            var expected = "test";
            
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);
            builder.UseRoutePrefix(expected);

            var webServiceType = builder.Build(0);
            Assert.That(webServiceType.RoutePrefix, Is.EqualTo(expected));
        }

        [Test]
        public void WebServiceBuilder_Service() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);

            var webServiceType = builder.Build(0);
            Assert.That(webServiceType.Service, Is.EqualTo(_serviceInstance));
        }

        [Test]
        public void WebServiceBuilder_ServiceFactory() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            Func<object> serviceFactory = () => _serviceInstance;
            builder.UseService(serviceFactory);

            var webServiceType = builder.Build(0);
            Assert.That(webServiceType.ServiceFactory, Is.EqualTo(serviceFactory));
        }

        [Test]
        public void WebServiceBuilder_UseRoute_NoServiceOrFactory() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            Assert.That(() => builder.UseRoute(nameof(MockWebService.StandardMethod)), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void WebServiceBuilder_UseRoute_NoAttribute() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);
            Assert.That(() => builder.UseRoute(nameof(MockWebService.StandardMethod)), Throws.ArgumentException);
        }
        
        [Test]
        public void WebServiceBuilder_UseRoute_Attribute_Pattern() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);
            builder.UseRoute(nameof(MockWebService.MethodWithPattern));
            var route = builder.Routes.First();

            Assert.That(route.Pattern, Is.EqualTo("method"));
        }
        
        [Test]
        public void WebServiceBuilder_UseRoute_Attribute_RootedPattern() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);
            builder.UseRoute(nameof(MockWebService.RootedMethod));
            
            var route = builder.Routes.First();
            Assert.That(route.IsRooted, Is.EqualTo(true));
        }
        
        [Test]
        public void WebServiceBuilder_UseRoute_Attribute_ContainsPattern() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);


            builder.UseRoute(nameof(MockWebService.MethodWithContains));
            var route = builder.Routes.First();

            Assert.That(route.MatchingStrategy, Is.EqualTo(RouteMatchingStrategy.Contains));
        }
        
        [Test]
        public void WebServiceBuilder_UseRoute_Attribute_RegexPattern() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);

            builder.UseRoute(nameof(MockWebService.MethodWithRegex));
            var route = builder.Routes.First();

            Assert.That(route.MatchingStrategy, Is.EqualTo(RouteMatchingStrategy.Regex));
        }
        
        [Test]
        public void WebServiceBuilder_UseRoute_Attribute_GetMethod() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);

            builder.UseRoute(nameof(MockWebService.MethodWithHttpGet));
            var route = builder.Routes.First();

            
            Assert.That(route.AcceptedMethods, Is.EqualTo(HttpMethods.Get));
        }
        
        [Test]
        public void WebServiceBuilder_UseRoute_Attribute_PostMethod() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);

            builder.UseRoute(nameof(MockWebService.MethodWithHttpPost));
            var route = builder.Routes.First();
            
            Assert.That(route.AcceptedMethods, Is.EqualTo(HttpMethods.Post));
        }
        
        [Test]
        public void WebServiceBuilder_UseMultipleRoute_Index() {
            var builder = new WebServiceBuilder(typeof(MockWebService));
            builder.UseService(_serviceInstance);

            builder.UseRoute(nameof(MockWebService.MethodWithContains));
            builder.UseRoute(nameof(MockWebService.RootedMethod));
            builder.UseRoute(nameof(MockWebService.MethodWithRegex));
            builder.UseRoute(nameof(MockWebService.MethodWithPattern));

            var webServiceType = builder.Build(0);

            var indices = webServiceType.Routes.Select(r => r.Route.RouteIndex).OrderBy(r => r).ToArray();

            CollectionAssert.AreEqual(new []{0,1,2,3}, indices);
        }

        private class MockWebService : WebServiceBase{
            public void StandardMethod(){}
            
            [HttpGet]
            [Route("method")]
            public void MethodWithPattern(){}
            
            [HttpGet]
            [Route("method", IsRooted = true)]
            public void RootedMethod(){}
            
            [HttpGet]
            [Route("method", RouteMatchingStrategy.Contains)]
            public void MethodWithContains(){}
            
            [HttpGet]
            [Route("method", RouteMatchingStrategy.Regex)]
            public void MethodWithRegex(){}
            
            [HttpGet]
            [Route("method")]
            public void MethodWithHttpGet(){}
            
            [HttpPost]
            [Route("method")]
            public void MethodWithHttpPost(){}
        }
    }
}
