namespace DotLogix.WebServices.Core.Terms; 

public class OrderingKey {
    public string Name { get; set; }
    public SortDirection Direction { get; set; }

    public static OrderingKey Ascending(string name) {
        return new OrderingKey {Name = name, Direction = SortDirection.Ascending};
    }
        
    public static OrderingKey Descending(string name) {
        return new OrderingKey {Name = name, Direction = SortDirection.Descending};
    }
        
    public static implicit operator OrderingKey(string name) {
        return Ascending(name);
    }
}