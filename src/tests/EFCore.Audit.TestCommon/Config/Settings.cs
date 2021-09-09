using System;
using System.IO;

namespace EFCore.Audit.TestCommon
{
    public class Settings
    {
        public bool IsDockerComposeRequired { get; set; }
        public string ConnectionString { get; set; }
        public string MigrationsAssembly { get; set; }
        public string DockerComposeFile { get; set; }
        public string DockerWorkingDir { get; set; }
        public string DockerComposeExePath { get; set; }
        public int DockerSleepInMs { get; set; }
        public bool IsGithubAction { get; set; }

        public Settings()
        {
            _ = bool.TryParse(Environment.GetEnvironmentVariable("IS_GITHUB_ACTION"), out bool IsGithubAction);
            
            this.IsGithubAction = IsGithubAction;
            this.DockerComposeFile = "docker-compose.yml";
            this.DockerComposeExePath = "C:\\ProgramData\\chocolatey\\bin\\docker-compose.exe";
            this.DockerSleepInMs = 30000;
            this.IsDockerComposeRequired = true;
            this.IsGithubAction = false;
            this.DockerWorkingDir = this.GetType().GetDir().GetParentDir().GetParentDir().GetParentDir();
            this.MigrationsAssembly = $"{this.DockerWorkingDir.GetDirName()}.Migrations";
        }
    }

    public static class Extensions
    {
        public static string GetDir(this Type type) => Path.GetDirectoryName(type.Assembly.Location);
        public static string GetParentDir(this string dir) => Directory.GetParent(dir).FullName;
        public static string GetDirName(this string dir) => new DirectoryInfo(dir).Name;
    }
}
