using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Linq;
using System.Windows;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        private readonly BillsContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Users> UsersRepository;
        private readonly IRepository<Companies> CompaniesRepository;

        public Orders(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            UsersRepository = unitOfWork.Repository<Users>();
            CompaniesRepository = unitOfWork.Repository<Companies>();
            InitializeComponent();
        }

        private void BtnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var userName = lblUserName.Content.ToString().Split(" ")[1];
            var selectedUser = UsersRepository.Find(u => u.UserName.ToLower()
            == userName.ToLower()).SingleOrDefault();
            var selectedCompany = CompaniesRepository.Get(selectedUser.CompanyId);

            CreateOrder createOrder = new(_context);
            createOrder.txtUsername.Text = userName;
            createOrder.txtCompanyName.Text = selectedCompany.Name;
            createOrder.Show();
        }

        private void BtnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            ViewOrders viewOrders = new(_context);
            viewOrders.lblUserName.Content = lblUserName.Content.ToString().Split(" ")[1];
            viewOrders.Show();
            Close();
        }
    }
}
