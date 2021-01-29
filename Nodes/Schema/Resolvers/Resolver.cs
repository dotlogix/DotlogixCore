namespace DotLogix.Core.Nodes.Schema {
    public class Resolver {
        public NamingStrategyProvider NamingStrategyProvider { get; set; } = new NamingStrategyProvider();
        public NodeConverterProvider NodeConverterProvider { get; set; } = new NodeConverterProvider();
        public NodeConverterFactoryProvider NodeConverterFactoryProvider { get; set; } = new NodeConverterFactoryProvider();
        
        
    }
}