namespace DotLogix.WebServices.EntityFramework.Database; 

public class DatabaseCommand {
    public string Name { get; }

    public DatabaseCommand(string name) {
        Name = name;
    }
}