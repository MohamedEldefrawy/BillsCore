using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
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
            using (SqlConnection sqlConnection = new(connectionString))
            {
                string filePath = GetDestinationPath("BillsCore.sql", @"Static");
                string script = File.ReadAllText(filePath);
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                     RegexOptions.Multiline | RegexOptions.IgnoreCase);

                sqlConnection.Open();
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, sqlConnection))
                        {
                            command.ExecuteNonQuery();

                        }
                    }
                }

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
