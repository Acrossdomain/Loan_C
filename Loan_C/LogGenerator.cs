using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTL_MSTR
{
    public class LogGenerator
    {
        public void WriteLog(string datefile,string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            //Today.ToString("yyyy-MM-dd h:mm tt")
            string logFilePath = @"Logs\\";
            //string datefile = System.DateTime.Now.ToString();
            //datefile = datefile.Replace(" ", "-");
            //datefile = datefile.Replace(":", "");
            //datefile = datefile.Replace("/","-");
            logFilePath = logFilePath + "Log- " + datefile + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();
        }
    }
}
