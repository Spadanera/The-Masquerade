using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace The_Masquerade.Models
{
    public class LogWriter
    {
        private string logFile { get; set; } // path + file name
        private int logLevel { get; set; }

        //The log file name if contain a %, it will be change with the current date
        public LogWriter(string _logFile, int _logLevel)
        {
            this.logFile = Path.Combine(HttpContext.Current.Server.MapPath("~/Logs"), 
                _logFile.Replace("%", DateTime.Now.ToString("yyyy-MM-dd")) + ".log");
            this.logLevel = _logLevel;
        }

        public void WriteLog(string text, int levelLog)
        {
            if (levelLog <= this.logLevel)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(this.logFile, true);
                    try
                    {
                        string logLine = DateTime.Now.ToString() + " - " + text;
                        writer.WriteLine(logLine);
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
                catch { }
            }
        }
    }

    public static class LogRetention
    {
        public static void LogCleaning(string path, int retention)
        {
            String dayToKeepBk = retention.ToString();
            String fileToFind = "*.log";

            ArrayList arrFile = new ArrayList();
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] rgFiles = di.GetFiles(fileToFind);
            foreach (FileInfo fi in rgFiles)
                arrFile.Add(fi.Name);

            // delete file old than today - 'dayToKeepBk'
            for (int i = 0; i < arrFile.Count; i++)
            {
                String myFileName = arrFile[i].ToString();
                FileInfo rarFile = new FileInfo(path + myFileName);

                DateTime dhms = DateTime.Now;
                DateTime dhmsFrom = dhms.AddDays(-int.Parse(dayToKeepBk));

                String sDhms = dhms.ToString("yyyyMMdd");
                String sDhmsFrom = dhmsFrom.ToString("yyyyMMdd");
                String sDhmsRar = rarFile.LastWriteTime.ToString("yyyyMMdd");

                if (int.Parse(sDhmsRar) < int.Parse(sDhmsFrom))
                {
                    File.Delete(path + myFileName);
                }
            }
        }
    }
}
