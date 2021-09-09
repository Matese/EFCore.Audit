using System;
using System.Diagnostics;
using System.Threading;

namespace EFCore.Audit.IntegrationTest.Docker
{
    public class DockerComposer : IDisposable
    {
        private Process _dockerProcess;
        public string DockerComposeExe { get; private set; }
        public string ComposeFile { get; private set; }
        public string WorkingDir { get; private set; }
        public int SleepInMs { get; private set; }

        public DockerComposer(string dockerComposeExe, string composeFile, string workingDir, int sleepInMs)
        {
            DockerComposeExe = dockerComposeExe;
            ComposeFile = composeFile;
            WorkingDir = workingDir;
            SleepInMs = sleepInMs;
        }

        public void Start()
        {
            var startInfo = GenerateInfo("up");
            _dockerProcess = Process.Start(startInfo);
            Thread.Sleep(SleepInMs);
        }

        public void Dispose()
        {
            _dockerProcess.Close();
            var stopInfo = GenerateInfo("down");
            var stop = Process.Start(stopInfo);
            stop.WaitForExit();
        }

        private ProcessStartInfo GenerateInfo(string argument)
        {
            var procInfo = new ProcessStartInfo
            {
                FileName = this.DockerComposeExe,
                Arguments = $"-f {this.ComposeFile} {argument}",
                WorkingDirectory = this.WorkingDir
            };

            return procInfo;
        }
    }
}
