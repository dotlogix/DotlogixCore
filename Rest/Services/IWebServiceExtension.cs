namespace DotLogix.Core.Rest.Services {
    public interface IWebServiceExtension {
        public string Name { get; }
        public void Configure(WebServiceSettings settings);
    }
}