using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;
using SmartFaceAuth.Models;

namespace SmartFaceAuth.FaceAuth
{
    public class Logger
    {
        bool WriteLogToFile = Convert.ToBoolean(WebConfigurationManager.AppSettings["WriteLogToFile"]);
        static StreamWriter Writer = new StreamWriter(WebConfigurationManager.AppSettings["LogFilePath"], true);
        DbApiConnector apiConnector = new DbApiConnector();

        public void WriteToFile(Log logs)
        {
            if (WriteLogToFile)
            {
                lock (Writer)
                {
                    Writer.WriteLine(string.Format("[{0}] Guid: {1}, Message: {2}", logs.Time, logs.UserGuid, logs.Message));
                    Writer.Flush();
                }
            }
        }

        public void WriteToFile(Exception exc)
        {
            if (WriteLogToFile)
            {
                lock (Writer)
                {
                    Writer.WriteLine(string.Format("[{0}] {1}", DateTime.Now, exc.Message));
                    Writer.Flush();
                }
            }
        }

        public void WriteToDatabase(Log logs)
        {
            apiConnector.AppendLog(logs);
            WriteToFile(logs);
        }
    }
}