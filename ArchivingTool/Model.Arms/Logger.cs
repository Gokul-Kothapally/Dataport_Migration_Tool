using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivingTool.Model.Arms
{
    public static class Logger
    {
        public static void Write(string message, string rms = null, string module = null, Guid? agencyKey = null, string logType = "INFO")
        {
            try
            {
                string appFolder = GetAppRoot(); // you can move GetAppRoot here too

                string logsRoot = Path.Combine(appFolder, "Logs");
                string logFolder;

                if (rms != null && module != null && agencyKey.HasValue)
                    logFolder = Path.Combine(logsRoot, rms, agencyKey.ToString(), module);
                else
                    logFolder = Path.Combine(logsRoot, "General");

                Directory.CreateDirectory(logFolder);
                string filePath = Path.Combine(logFolder, $"{logType}_Log_{DateTime.Now:yyyy_MM_dd}.txt");

                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logType}]  {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Log error: " + ex.Message);
            }
        }

        public static string GetAppRoot()
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            string current = Path.GetDirectoryName(exePath);

            // If running inside bin/, walk up
            if (current.Contains(Path.Combine("bin", "")))
                return Directory.GetParent(current)?.Parent?.Parent?.FullName;

            return current;
        }
    }

}
