using System;
using System.IO;

namespace Oyooni.Server.Common
{
    public class DisposableTempFile : IDisposable
    {
        protected string _pathToTempFile;

        public string PathToTempFile { get; }

        public DisposableTempFile(string pathToTempFile)
        {
            _pathToTempFile = pathToTempFile;
        }

        public void Dispose()
        {
            if (File.Exists(_pathToTempFile)) File.Delete(_pathToTempFile);
        }
    }
}
