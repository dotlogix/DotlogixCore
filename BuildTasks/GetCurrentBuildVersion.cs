#region
using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
#endregion

namespace BuildTasks {
    public class GetCurrentBuildVersion : Task {
        [Output]
        public string PackageVersion { get; set; }

        [Output]
        public string BuildVersion { get; set; }

        public string BaseVersion { get; set; }
        public string PackageId { get; set; }

        public override bool Execute() {
            PackageVersion = BaseVersion;
            BuildVersion = BaseVersion;

            //var originalVersion = Version.Parse(BaseVersion ?? "1.0.0");
            //if(PackageId != null) {
            //    var solutionDir = typeof(GetCurrentBuildVersion).Assembly.Location;
            //    for (var i = 0; i < 3; i++)
            //        solutionDir = Path.GetDirectoryName(solutionDir);
            //    Console.WriteLine($"SolutionDir = {solutionDir}");

            //    if (solutionDir != null)
            //    {
            //        var buildFile = Path.Combine(solutionDir, "build.json");
            //        var rootNode = JsonNodes.ToNode<NodeList>(File.ReadAllText(buildFile));

            //        var projectNode = rootNode.Children<NodeMap>().FirstOrDefault(c => c.GetChildValue<string>("packageId") == PackageId);
            //        if (projectNode != null && projectNode.TryGetChildValue("version", out string version)) {
            //            Console.WriteLine($"Override {PackageId} original version {originalVersion} with version {version}");
            //            originalVersion = System.Version.Parse(version);
            //        }
            //    }
            //}

            //PackageVersion = originalVersion.ToString();
            //BuildVersion = GetCurrentBuildVersionString(originalVersion).ToString();
            return true;
        }

        private static Version GetCurrentBuildVersionString(Version baseVersion) {
            var utcNow = DateTime.UtcNow;
            var build = (utcNow.Date - new DateTime(2000, 1, 1)).Days;
            var version = new Version(baseVersion.Major, baseVersion.Minor, Math.Max(0, baseVersion.Build), build);
            return version;
        }
    }
}
