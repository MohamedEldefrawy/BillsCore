using com.rusanu.DBUtil;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace BillsDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string connectionString = "server=.; database=BillsCore; Trusted_Connection=true";

        public App()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string filePath = GetDestinationPath("BillsCore.sql", @"Static");

                var inst = new SqlCmd(sqlConnection);
                inst.ExecuteFile(filePath);
            }
        }

        private static string GetDestinationPath(string filename, string foldername)
        {
            string appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = string.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }
    }
}
