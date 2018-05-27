namespace DotLogix.Core.Config {
    public interface IConfigurationFile<TConfig> {
        string FileName { get; }
        string Directory { get; }
        string AbsolutePath { get; }
        TConfig CurrentConfig { get; }
        bool TryLoad();
        bool TrySave();
    }
}