using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class LogTest : DomainEntity
    {
        private string _path;
        private string _fileName;
        public LogTest(string name, int maxRetries, int restartTimeout,String path,String filename) : base(name, maxRetries, restartTimeout)
        {
            this._fileName = ValidateFileName(filename);
            this._path = ValidateDirectory(path);
        }
        private static string ValidateDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Pfad darf nicht leer sein.", nameof(path));

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Ordner existiert nicht: {path}");

            if (File.Exists(path))
                throw new ArgumentException($"Der Pfad verweist auf eine Datei, nicht auf einen Ordner: {path}");

            return path;
        }

        private static string ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Dateiname darf nicht leer sein.", nameof(fileName));

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException($"Ungültiger Dateiname: {fileName}", nameof(fileName));

            return fileName;
        }


        public override ExecutionResult IsHealthy()
        {
            throw new NotImplementedException();
        }

       /* public override Task<ExecutionResult> RecoverAsync()
        {
            throw new NotImplementedException();
        }*/

        public override ExecutionResult StartAsync()
        {
            throw new NotImplementedException();
        }

        public override ExecutionResult Stop()
        {
            throw new NotImplementedException();
        }
        public override ExecutionResult Recover()
        {
            throw new NotImplementedException();
        }
    }
}
