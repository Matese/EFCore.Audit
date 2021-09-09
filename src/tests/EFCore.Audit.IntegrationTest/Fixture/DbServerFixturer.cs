using EFCore.Audit.IntegrationTest.Config;
using EFCore.Audit.IntegrationTest.Docker;
using EFCore.Audit.TestCommon;
using System;

namespace EFCore.Audit.IntegrationTest.Fixture
{
    public abstract class DbServerFixturer : IDisposable
    {
        public Settings Settings { get; private set; }
        
        private readonly DockerComposer _dockerStarter;
        private bool _disposed;

        public DbServerFixturer()
        {
            this.Settings = SettingsGetter.Get();

            if (this.Settings.IsDockerComposeRequired && !this.Settings.IsGithubAction)
            {
                _dockerStarter = new DockerComposer(
                    this.Settings.DockerComposeExePath,
                    this.Settings.DockerComposeFile,
                    this.Settings.DockerWorkingDir,
                    this.Settings.DockerSleepInMs);

                _dockerStarter.Start();
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (this.Settings.IsDockerComposeRequired && !this.Settings.IsGithubAction)
                    {
                        _dockerStarter.Dispose();
                    }
                }

                _disposed = true;
            }
        }
    }
}
