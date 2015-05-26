using System;
using Common.Logging;

namespace Com.Meten.WeChatSmeten.Helper
{
    public class LogHelper
    {
        private static ILog ilog;

        private static ILog DefaultILog
        {
            get
            {
                try
                {
                    return ilog ?? (ilog = LogManager.GetLogger("root"));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 写Info日志
        /// </summary>
        private static void Info(object message)
        {
            if (DefaultILog.IsInfoEnabled)
                DefaultILog.Info(message);
        }

        /// <summary>
        /// 写Debug日志
        /// </summary>
        private static void Debug(object message)
        {
            if (DefaultILog.IsDebugEnabled)
                DefaultILog.Debug(message);
        }

        /// <summary>
        /// 写Warn日志
        /// </summary>
        private static void Warn(object message)
        {
            if (DefaultILog.IsWarnEnabled)
                DefaultILog.Warn(message);
        }

        /// <summary>
        /// 写Error日志
        /// </summary>
        private static void Error(object message)
        {
            if (DefaultILog.IsErrorEnabled)
                DefaultILog.Error(message);
        }

        /// <summary>
        /// 写Fatal日志
        /// </summary>
        private static void Fatal(object message)
        {
            if (DefaultILog.IsFatalEnabled)
                DefaultILog.Fatal(message);
        }



        /// <summary>
        /// 写Info日志
        /// </summary>
        /// <param name="message">日记内容</param>
        /// <param name="logName">记录器名称;如不填,则默认为root</param>
        public static void WriteInfo(object message, string logName="")
        {
            if (string.IsNullOrEmpty(logName) || logName == "root")
                Info(message);
            else
            {
                ILog logWrite = GetLoger(logName);
                if (logWrite.IsInfoEnabled)
                    logWrite.Info(message);
            }
        }

        /// <summary>
        /// 写Debug日志
        /// </summary>
        /// <param name="message">日记内容</param>
        /// <param name="logName">记录器名称;如不填,则默认为root</param>
        public static void WriteDebug(object message, string logName = "")
        {
            if (string.IsNullOrEmpty(logName) || logName == "root")
                Debug(message);
            else
            {
                ILog logWrite = GetLoger(logName);
                if (logWrite.IsDebugEnabled)
                    logWrite.Debug(message);
            }
        }

        /// <summary>
        /// 写Warn日志
        /// </summary>
        /// <param name="message">日记内容</param>
        /// <param name="logName">记录器名称;如不填,则默认为root</param>
        public static void WriteWarn(object message, string logName = "")
        {
            if (string.IsNullOrEmpty(logName) || logName == "root")
                Warn(message);
            else
            {
                ILog logWrite = GetLoger(logName);
                if (logWrite.IsWarnEnabled)
                    logWrite.Warn(message);
            }
        }

        /// <summary>
        /// 写Error日志
        /// </summary>
        /// <param name="message">日记内容</param>
        /// <param name="logName">记录器名称;如不填,则默认为root</param>
        public static void WriteError(object message, string logName = "")
        {
            if (string.IsNullOrEmpty(logName) || logName == "root")
                Error(message);
            else
            {
                ILog logWrite = GetLoger(logName);
                if (logWrite.IsErrorEnabled)
                    logWrite.Error(message);
            }
        }

        /// <summary>
        /// 写Fatal日志
        /// </summary>
        /// <param name="message">日记内容</param>
        /// <param name="logName">记录器名称;如不填,则默认为root</param>
        public static void WriteFatal(object message, string logName = "")
        {

            if (string.IsNullOrEmpty(logName) || logName == "root")
                Fatal(message);
            else
            {
                ILog logWrite = GetLoger(logName);
                if (logWrite.IsFatalEnabled)
                    logWrite.Fatal(message);
            }
        }

        /// <summary>
        /// 获取日志输出者
        /// </summary>
        public static ILog GetLoger(string name)
        {
            return LogManager.GetLogger(name);
        }


        private static ILog fileLog;

        public static ILog FileLog
        {
            get
            {
                try
                {
                    return fileLog ?? (fileLog = LogManager.GetLogger("file"));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}