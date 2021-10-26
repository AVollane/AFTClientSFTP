using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace AFTClient
{
    class Program
    {
        static string remoteDirectory = ConfigurationManager.AppSettings.Get("RemoteDirectory");
        static string savingDirectory = ConfigurationManager.AppSettings.Get("SavingDirectory");
        static string logFilePath = ConfigurationManager.AppSettings.Get("LogFilePath");
        static int connectionRecurrences;
        static int connectionsMillisecondsDelay;
        static Logger logger;
        static Receiver receiver;

        static void Main(string[] args)
        {
            logger = new Logger(logFilePath);
            receiver = GetConfiguratedReceiver();
            Int32.TryParse(ConfigurationManager.AppSettings.Get("ConnectionRecurrences"), out connectionRecurrences);
            Int32.TryParse(ConfigurationManager.AppSettings.Get("ConnectionsMillisecondsDelay"), out connectionsMillisecondsDelay);
            TryConnect(connectionRecurrences, connectionsMillisecondsDelay);
            if (!receiver.IsConnected)
                return;
            receiver.DownloadFiles(remoteDirectory, savingDirectory);
            receiver.Disconnect();
        }

        /// <summary>
        /// Проводим несколько попыток подключения
        /// </summary>
        /// <param name="recurrences"></param>
        /// <param name="millisecondsDelay"></param>
        private static void TryConnect(int recurrences, int millisecondsDelay)
        {
            if (recurrences == 0)
                recurrences = 5;
            if (millisecondsDelay == 0)
                millisecondsDelay = 5000;

            for(int i = 0; i < recurrences; i++)
            {
                try
                {
                    receiver.Connect();
                    break;
                }
                catch(Exception)
                {
                    Thread.Sleep(millisecondsDelay);
                    Console.WriteLine("Unable to connect. . .");
                }
            }
        }

        /// <summary>
        /// Возвращает сконфигурированный приемник
        /// </summary>
        /// <returns></returns>
        static Receiver GetConfiguratedReceiver()
        {
            string host = ConfigurationManager.AppSettings.Get("Host");
            string username = ConfigurationManager.AppSettings.Get("Username");
            string password = ConfigurationManager.AppSettings.Get("Password");
            return new Receiver(host, username, password);
        }
    }
}
