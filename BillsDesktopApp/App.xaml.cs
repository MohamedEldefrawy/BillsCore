using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private const string connectionString = "server=.;Trusted_Connection=true";

        public App()
        {
            using (SqlConnection sqlConnection = new(connectionString))
            {
                string filePath = Environment.CurrentDirectory + @"\Static\BillsCore.sql";
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
    }
}
