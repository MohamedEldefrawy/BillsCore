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
           
        }       
    }
}
