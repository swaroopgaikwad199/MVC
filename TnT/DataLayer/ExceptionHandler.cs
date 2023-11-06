using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TnT.DataLayer
{
    public class ExceptionHandler
    {
        public static class ExceptionLogger
        {
            public static void logException(Exception ex)
            {
                Task.Run(() => { Logger.noteException(ex); });
            }

            public static void LogError(string ex)
            {
                Task.Run(() => { Logger.noteString(ex); });
            }
        }

        class Logger
        {
            public static void noteException(Exception ex)
            {
                string filePath = Utilities.getAppSettings("ErrorHandle");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if(!File.Exists(filePath+"\\Exception.txt"))
                {
                    File.Create(filePath + "\\Exception.txt");
                }
                using (StreamWriter writer = new StreamWriter(filePath + "\\Exception.txt", true))
                {
                    writer.WriteLine(Environment.NewLine + "----------------------------------------------------------------------------------------------------------" + Environment.NewLine);
                    writer.WriteLine(Environment.NewLine + "---------------------------------[| " + DateTime.Now.ToString() + " |]------------------------------------" + Environment.NewLine);
                    writer.WriteLine("Message : " + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace + "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "---------------------------------------------[| END |]----------------------------------------------------" + Environment.NewLine);
                    writer.Close();
                }
            }

            public static void noteString(string ex)
            {
                string filePath = Utilities.getAppSettings("ErrorHandle");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!File.Exists(filePath + "\\Exception.txt"))
                {
                    File.Create(filePath + "\\Exception.txt");
                }
                using (StreamWriter writer = new StreamWriter(filePath + "\\Exception.txt", true))
                {
                    writer.WriteLine(Environment.NewLine + "----------------------------------------------------------------------------------------------------------" + Environment.NewLine);
                    writer.WriteLine(Environment.NewLine + "---------------------------------[| " + DateTime.Now.ToString() + " |]------------------------------------" + Environment.NewLine);
                    writer.WriteLine("Message : " + ex + "<br/>" + Environment.NewLine +Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "---------------------------------------------[| END |]----------------------------------------------------" + Environment.NewLine);
                    writer.Close();
                }
            }
        }


    }
}