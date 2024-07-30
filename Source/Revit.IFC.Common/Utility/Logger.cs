using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;

namespace Revit.IFC.Common.Utility
{
   public class Logger
   {
      private ILog _internalLogger;

      private static RollingFileAppender FileAppender
      {
         get
         {
            if (null == _fileAppender)
            {
               _fileAppender = new RollingFileAppender();
               _fileAppender.File = LogFilePath;
               _fileAppender.MaxSizeRollBackups = 10;
               _fileAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
               _fileAppender.DatePattern = "_dd-MM-yyyy";
               _fileAppender.MaximumFileSize = "10MB";
               _fileAppender.ActivateOptions();
               _fileAppender.AppendToFile = true;
               _fileAppender.Encoding = Encoding.UTF8;
               _fileAppender.Layout = new log4net.Layout.XmlLayoutSchemaLog4j();
               _fileAppender.ActivateOptions();
            }

            return _fileAppender;
         }
      }
      private static RollingFileAppender _fileAppender;
      
      private static string LogFilePath =>
         _logFilePath ?? (_logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            "Paradigm", "Transformer" + " " + 2023, "Log", "Revit.log"));

      private static string _logFilePath;

      public static Logger CreateLogger(Type type)
      {
         string name = type.ToString();
         Logger logger = new Logger();
         ILoggerRepository repository = LogManager.GetRepository(name);

         if (null == repository)
         {
            repository = LogManager.CreateRepository(name);
            BasicConfigurator.Configure(repository, FileAppender);
         }
         
         logger._internalLogger = LogManager.GetLogger(name, type);
         
         return logger;
      }
      
      [MethodImpl(MethodImplOptions.NoInlining)]
      internal static string GetCurrentMethod()
      {
         StackTrace st = new StackTrace();
         StackFrame sf = st.GetFrame(1);
         return sf.GetMethod().Name;
      }

      public void Log(Exception ex)
      {
         _internalLogger?.Error("Error", ex);
      }

      public void Log(string text, Exception ex)
      {
         _internalLogger?.Error(text, ex);
      }

      public void Log(string text)
      {
         _internalLogger?.Info(text);
      }

      public void Error(string text)
      {
         _internalLogger?.Error(text);
      }

      public void Warn(string text)
      {
         _internalLogger?.Warn(text);
      }

      public void Warn(string text, Exception ex)
      {
         _internalLogger?.Warn(text, ex);
      }

      public void Info(string text)
      {
         _internalLogger?.Info(text);
      }
   }
}