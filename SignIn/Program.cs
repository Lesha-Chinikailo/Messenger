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
            string host = Dns.GetHostName();;
            IPAddress[] addresses = Dns.GetHostAddresses(host);

            if (addresses[0].AddressFamily != AddressFamily.InterNetwork)
            {
                MessageBox.Show("you are not connected to the network! connect and restart the application", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new SignIn());
        }
    }
}