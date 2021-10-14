using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFTClient
{
    public class Receiver
    {
        private SftpClient _client;
        private string _host;
        private string _username;
        private string _password;
        private Logger _logger;

        public Receiver(string host, string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;
            _client = new SftpClient(_host, _username, _password);
            string logFilePath = ConfigurationManager.AppSettings.Get("LogFilePath");
            _logger = new Logger(logFilePath);
        }

        public void Connect() => _client.Connect();
        public void Disconnect() => _client.Disconnect();

        public void DownloadFiles(string remoteDirectory, string localDirectory)
        {
            if (localDirectory.EndsWith('/'))
                localDirectory += @"/";
            if (remoteDirectory.EndsWith('/'))
                remoteDirectory += @"/";

            // Файлы, которые начинаются с точки, являются системными. Мы не должны их скачивать,
            // по этому фильтруем названия
            IEnumerable<SftpFile> files = _client.ListDirectory(remoteDirectory).Where(x => !x.Name.StartsWith('.'));

            foreach(SftpFile file in files)
            {
                using (Stream stream = File.OpenWrite($"{localDirectory}/{file.Name}"))
                {
                    try
                    {
                        _client.DownloadFile(remoteDirectory + file.Name, stream);
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                }
            }
        }
    }
}
