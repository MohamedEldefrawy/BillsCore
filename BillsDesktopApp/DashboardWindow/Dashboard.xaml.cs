using BillsDesktopApp.AuthWindows;
using BillsDesktopApp.CustomersWindows;
using BillsDesktopApp.ProductsWindows;
using BillsDesktopApp.OrdersWindows;
using DAL;
using DAL.UnitOfWork;
using System.Windows;

namespace BillsDesktopApp.DashboardWindow
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        public Dashboard(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ChangeProfile changeProfile = new ChangeProfile(_context);
            changeProfile.txtUserName.Text = lblWelcome.Content.ToString().Split(" ")[2];
            changeProfile.Owner = Window.GetWindow(this);
            changeProfile.ShowDialog();
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            Customers customers = new Customers(_context);
            customers.lblUserName.Content = "مرحباً " + lblWelcome.Content.ToString().Split(" ")[2];
            customers.Owner = Window.GetWindow(this);
            customers.ShowDialog();
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            winProducts products = new winProducts(_context);
            products.lblUserName.Content = "مرحباً " + lblWelcome.Content.ToString().Split(" ")[2];
            products.Owner = Window.GetWindow(this);
            products.ShowDialog();
        }

        private void btnBills_Click(object sender, RoutedEventArgs e)
        {
            Orders orders = new Orders(_context);
            orders.lblUserName.Content = "مرحباً " + lblWelcome.Content.ToString().Split(" ")[2];
            orders.Owner = Window.GetWindow(this);
            orders.ShowDialog();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("هل انت متأكد ؟", "تأكيد تسجيل الخروج", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }
    }
}
