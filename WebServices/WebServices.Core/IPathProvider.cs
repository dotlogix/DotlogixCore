namespace DotLogix.WebServices.Core {
    public interface IPathProvider {
        string BinDirectory { get; }
        string LogDirectory { get; }
        string DataDirectory { get; }
        string TempDirectory { get; }
        string RootDirectory { get; }
        string ToAbsolutePath(params string[] paths);
        string ToAbsolutePath(string path1, string path2);
        string ToAbsolutePath(string path1, string path2, string path3);
        string EnsureDirectory(string absolutePath);
    }
}
