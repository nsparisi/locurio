using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class Debug
    {
        private const string NullString = "<null>";
        private const string LogFormat = "[{0}][{1}] {2}{3}";
        private const string LogTimeFormat = "MM/dd H:mm:ss.ffff";

        private static string logFile;
        private static bool initialized;
        private static object threadLock = new Object();

        public static event AbyssEvent LogMessageEvent;

        public class LogEventArgs : EventArgs
        {
            public string Message { get; private set; }
            public DateTime Timestamp { get; private set; }
            public LogEventArgs(string message)
            {
                this.Message = message;
                this.Timestamp = DateTime.Now;
            }
        }

        private static void Initialize()
        {
            if (!initialized)
            {
                initialized = true;
                logFile = string.Format("Abyss_{0}.log", DateTime.Now.ToString("MM-dd_H-mm-ss"));
                logFile = Path.Combine(System.Environment.CurrentDirectory, "Logs", logFile);

                if (!Directory.Exists(Path.GetDirectoryName(logFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(logFile));
                }
            }
        }

        public static void Log(string message, params object[] parameters)
        {
            HandleLog(string.Format(message, parameters));
        }
        
        public static void Log(object objectToPrint)
        {
            HandleLog(objectToPrint == null ? NullString : objectToPrint.ToString());
        }
        
        public static void Log(string message, Exception e)
        {
            Log("{0}{1}{2}{1}{3}{1}",
                message, Environment.NewLine, e.Message, e.StackTrace);
        }

        private static void HandleLog(string message)
        {
            lock (threadLock)
            {
                Initialize();
                Console.Out.WriteLine(message);

                // ignore errors if writing to the log.
                // i've seen this happen with "no permission" error before. ??
                try
                {
                    File.AppendAllText(logFile, FormatLine(message));
                }
                catch
                {
                    
                }

                if(LogMessageEvent != null)
                {
                    LogMessageEvent(null, new LogEventArgs(message));
                }
            }
        }

        private static string FormatLine(string message)
        {
            return string.Format(
                LogFormat, 
                DateTime.Now.ToString(LogTimeFormat),
                Thread.CurrentThread.ManagedThreadId.ToString("0000000000"),
                message, 
                Environment.NewLine);
        }
    }
}
