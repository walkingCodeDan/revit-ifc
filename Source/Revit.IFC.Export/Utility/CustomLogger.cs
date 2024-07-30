using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Revit.IFC.Export.Utility
{
   public class CustomLogger
   {
      private static string LogFilePath =>
         _logFilePath ?? (_logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Revit_2023.log"));

      private static string _logFilePath;
      private string _name;

      public static CustomLogger CreateLogger(Type type)
      {
         CustomLogger customLogger = new CustomLogger();
         customLogger._name = type.ToString();
         
         return customLogger;
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
         WriteLog("ERROR", ex);
      }

      public void Log(string text)
      {
         WriteLog("LOG", text);
      }

      public void Error(string text)
      {
         WriteLog("ERROR", text);
      }

      public void Warn(string text)
      {
         WriteLog("WARN", text);
      }

      public void Info(string text)
      {
         WriteLog("INFO", text);
      }

      private void WriteLog(string logType, object obj)
      {
         File.AppendAllText(LogFilePath, 
            string.Format("{0}-{1}-{2}:{3}", 
               DateTime.Now.ToString("MM/dd/yyyy-hh:mm:ss"), logType, _name, obj));
      }
   }
}