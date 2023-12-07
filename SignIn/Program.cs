using System.Configuration;
using System.Net.Sockets;
using System.Net;

namespace SignIn
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string host = Dns.GetHostName();
            Console.WriteLine($"name laptop: {host}");
            IPAddress[] addresses = Dns.GetHostAddresses(host);

            if (addresses[0].AddressFamily != AddressFamily.InterNetwork)
            {
                MessageBox.Show("you are not connected to the network! connect and restart the application", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
            else
            {
                if (addresses[0].ToString() != ConfigurationManager.AppSettings.Get("serverIp"))
                {
                    MessageBox.Show("You are not connected to the server. When running, real-time messaging will not work. Errors may occur. I advise you to reconnect to the server.");
                }
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new SignIn());
        }
    }
}