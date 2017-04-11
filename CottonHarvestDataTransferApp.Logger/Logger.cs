using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Hammock;

namespace CottonHarvestDataTransferApp.Logging
{
    /// <summary>
    /// This is a simple logger class used to record.  Diagnostic information in the application.
    /// </summary>
    public static class Logger
    {
        private static string _fileName = "log.txt";
        private static string _path = "";
        private static string _locker = "";

        public static void SetLogPath(string path)
        {
            lock (_locker)
            {
                _path = path;
            }
        }

        public static void Log(string type, string message)
        {
            lock (_locker)
            {
                try
                {
                    string contents = string.Format("{0} {1} {2}\r\n", DateTime.Now.ToString(), type.PadRight(10), message);
                    File.AppendAllText(_path.TrimEnd('\\') + "\\" + _fileName, contents);
                }
                catch (Exception exc)
                {
                    //do nothing we just want to make sure the logger never creates an exception
                }
            }
        }
               

        public static void Log(RestResponse resp)
        {
            Log("RESPONSE", string.Format("STATUS CODE: {0}", resp.StatusCode));

            if (resp.TimedOut)
            {
                Log("RESPONSE", "RESPONSE TIMED OUT");
            }
        }

        public static string CurrentLogFile {
            get
            {
                return _path.TrimEnd('\\') + "\\" + _fileName;
            }
        }

        public static void Log(Exception exc)
        {
            Log("ERROR", exc.Message);
            Log("TRACE", exc.StackTrace);
        }

        public static void CleanUp()
        {
            lock (_locker)
            {
                string filepath = _path.TrimEnd('\\') + "\\" + _fileName;
                if (File.Exists(filepath))
                {
                    FileInfo info = new FileInfo(filepath);

                    if (info.Length > (1024 * 1024 * 15)) //truncate log file if it exceeds 15MB
                    {
                        File.Delete(filepath);
                    }
                }
            }
        }
    }
}
