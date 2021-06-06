using BillsDesktopApp.AuthWindows;
using BillsDesktopApp.ProductsWindows;
using DAL;
using System.Windows;
using DAL.Repositories.Origin;
using BL.Models;
using DAL.UnitOfWork;
using System.Linq;

namespace BillsDesktopApp.DashboardWindow
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private readonly BillsContext _context;
        private readonly IRepository<Users> UsersRepository;
        private readonly IRepository<Companies> CompaniesRepository;
        private readonly IUnitOfWork unitOfWork;

        public Dashboard(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            UsersRepository = unitOfWork.Repository<Users>();
            CompaniesRepository = unitOfWork.Repository<Companies>();
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersRepository.Get(int.Parse(lblUserId.Content.ToString()));
            var selectedCompany = CompaniesRepository.Find(c => c.ID == selectedUser.CompanyId).SingleOrDefault();

            ChangeProfile changeProfile = new ChangeProfile(_context);
            changeProfile.txtUserName.Text = lblWelcome.Content.ToString().Split(" ")[2];
            changeProfile.txtCompanyName.Text = selectedCompany.Name.ToString();
            changeProfile.txtTaxNumber.Text = selectedCompany.TaxNumber.ToString();
            changeProfile.Owner = GetWindow(this);
            changeProfile.ShowDialog();
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            CustomersWindows.Customers customers = new CustomersWindows.Customers(_context);
            customers.lblUserName.Content = "مرحباً " + lblWelcome.Content.ToString().Split(" ")[2];
            customers.Owner = GetWindow(this);
            customers.ShowDialog();
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            winProducts products = new winProducts(_context);
            products.lblUserName.Content = "مرحباً " + lblWelcome.Content.ToString().Split(" ")[2];
            products.Owner = GetWindow(this);
            products.ShowDialog();
        }

        private void btnBills_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindows.Orders orders = new OrdersWindows.Orders(_context);
            orders.lblUserName.Content = "مرحباً " + lblWelcome.Content.ToString().Split(" ")[2];
            orders.Owner = GetWindow(this);
            orders.ShowDialog();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("هل انت متأكد ؟", "تأكيد تسجيل الخروج", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                Close();
            }
        }
    }
}
