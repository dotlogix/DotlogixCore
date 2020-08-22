namespace DotLogix.Core.Rest.Services {
    public interface IRedirectWebServiceResult : IWebServiceResult {
        public string RedirectTo { get; set; }
        public bool Permanent { get; set; }
        public bool PreserveMethod { get; set; }
    }
}