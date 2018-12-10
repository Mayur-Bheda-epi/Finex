using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Collections;

namespace Finex.Logging.Log
{
    public enum LogDistribution { General, Email, EventLog, Scheduler, CouponService };
    /// <summary>
    /// Summary description for LogWriter.
    /// </summary>
    [Serializable]
   public class LogWriter
    {
        private static string[] categoryTypeName = new string[] { "General", "Email", "EventLog", "Scheduler", "CouponService" };
        private static object syncObject = new object();
        private string logCategory;
        private Hashtable extendedInformation;
        public string LogCategory
        {
            get { return logCategory; }
            set { logCategory = value; }
        }
        public Hashtable ExtendedInformation
        {
            get { return extendedInformation; }
            set { extendedInformation = value; }
        }

        public LogWriter()
        {
            logCategory = categoryTypeName[(int)LogDistribution.General];
            extendedInformation = new Hashtable();
        }
        /// <summary>
        /// Init a logcontroller with a specific Category
        /// </summary>
        /// <param name="categoryType"></param>
        public LogWriter(LogDistribution categoryType)
        {
            logCategory = categoryTypeName[(int)categoryType];
        }
        public LogWriter(Type classPosition)
            : this()
        {
            extendedInformation.Add("Position", classPosition.FullName);
        }
        public LogWriter(Type classPosition, string method)
            : this()
        {
            extendedInformation.Add("Position", classPosition.FullName + "." + method);
        }

        


        public void DebugLogging(string logMessage)
        {
            string fileName = @"Debug\Debug-" + DateTime.Now.ToString("MMddyy") + ".txt";
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            fileName = Path.GetFullPath(fileName);
            string directoryName = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            //lock(syncObject) // synchronyze write to log file
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss: ms ") + logMessage);
                        writer.WriteLine("---------------------");
                        writer.Flush();
                    }
                }
                return;
            }
        }

        public void ErrorLogging(string logMessage)
        {
            string fileName = @"Error\Error-" + DateTime.Now.ToString("MMddyy") + ".log";
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            fileName = Path.GetFullPath(fileName);
            string directoryName = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            //lock(syncObject) // synchronyze write to log file
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss: ms ") + logMessage);
                        writer.WriteLine("---------------------");
                        writer.Flush();
                    }
                }
                return;
            }
        }

        /// <summary>
        /// Function for writing Log
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="developer"></param>
        /// <param name="logType"></param>
        public void WriteLog(string logMessage, LogType logType)
        {

            switch (logType)
            {
                case LogType.Debug:
                    DebugLogging(logMessage);
                    break;
                case LogType.Error:
                    ErrorLogging(logMessage);
                    break;
            }
        }
          
      

        public enum LogType
        {
            /// <summary>
            /// 
            /// </summary>
            Error,
            Log,
            Debug
        }

        
    }
}
