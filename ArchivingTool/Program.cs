using Microsoft.Extensions.Configuration;
using System;
using System.Windows.Forms;

namespace ArchivingTool
{
    internal static class Program
    {
        public static IConfiguration? Configuration;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}