using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFTClient
{
    public class Logger
    {
        private string _logFilePath;
        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }
        public void Log(string message)
        {

        }
    }
}
