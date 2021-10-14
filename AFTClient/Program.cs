using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace AFTClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string remoteDirectory = ConfigurationManager.AppSettings.Get("RemoteDirectory");
            string savingDirectory = ConfigurationManager.AppSettings.Get("SavingDirectory");
            string logFilePath = ConfigurationManager.AppSettings.Get("LogFilePath");
            Logger logger = new Logger(logFilePath);
            Receiver receiver;
            try
            {
                receiver = GetConfiguratedReceiver();
                receiver.Connect();
                receiver.DownloadFiles(remoteDirectory, savingDirectory);
                receiver.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Log(ex.Message);
            }
        }

        static Receiver GetConfiguratedReceiver()
        {
            string host = ConfigurationManager.AppSettings.Get("Host");
            string username = ConfigurationManager.AppSettings.Get("Username");
            string password = ConfigurationManager.AppSettings.Get("Password");
            return new Receiver(host, username, password);
        }
    }
}
