using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class LogHelper
    {

        //private static void ChangeLog4netLogFileName(log4net.ILog iLog, string fileName)
        //{
        //    log4net.Core.LogImpl logImpl = iLog as log4net.Core.LogImpl;
        //    if (logImpl != null)
        //    {
        //        log4net.Appender.AppenderCollection ac = ((log4net.Repository.Hierarchy.Logger)logImpl.Logger).Appenders;
        //        for (int i = 0; i < ac.Count; i++)
        //        {     // 这里我只对RollingFileAppender类型做修改 
        //            log4net.Appender.RollingFileAppender rfa = ac[i] as log4net.Appender.RollingFileAppender;
        //            if (rfa != null)
        //            {
        //                rfa.File = fileName;
        //                if (!System.IO.File.Exists(fileName))
        //                {
        //                    System.IO.File.Create(fileName);
        //                }
        //                // 更新Writer属性 
        //                rfa.Writer = new System.IO.StreamWriter(rfa.File, rfa.AppendToFile, rfa.Encoding);
        //            }
        //        }
        //    }
        //}
        static log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
        public static void Info(String Name, String Message, string DeviceId)
        {


            appender.File = string.Format("D:/logs/{0}_{1}.log", DeviceId, DateTime.Now.ToString("yyyyMMdd"));
            appender.ActivateOptions();
            appender.AppendToFile = true;
            appender.Writer.WriteLine(String.Format("{0},{1}-\r\n{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Name, Message));

           
        }



        public static void Error(String Name, String Message)
        {

            log4net.LogManager.GetLogger(Name).Error(Message);

        }
        public static void Error(String Name, String Message,Exception ex)
        {
            
            log4net.LogManager.GetLogger(Name).Error(Message,ex);

        }
        public static void Info(String Name, String Message)
        {
            log4net.LogManager.GetLogger(Name).Info(Message);

        }

        public static void Debug(String Name, String Message)
        {
            log4net.LogManager.GetLogger(Name).Debug(Message);
        }

        public static void Warn(String Name, String Message)
        {
            log4net.LogManager.GetLogger(Name).Warn(Message);
        }
    }
}
