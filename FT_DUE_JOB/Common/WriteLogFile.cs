using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PU_EmiReminder_Due.Common
{
    public static class WriteLogFile
    {
        public static void LogToFile(string message)
        {
            try
            {
                string logDirectory = ConfigurationManager.AppSettings["LogPath"];
                //string logDirectory = @"D:\JC_Project\ConsoleApplication\PU_EmiReminder_Due\Logs";
                Directory.CreateDirectory(logDirectory);
                DateTime dateTime = DateTime.Now;
                string date =Convert.ToDateTime(dateTime).ToString("dd-MM-yyyy");
                string filename = "email_log_" + date + ".txt";
                string logFilePath = Path.Combine(logDirectory, filename);
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logging failed: " + ex.Message);
            }
        }

    }
}
