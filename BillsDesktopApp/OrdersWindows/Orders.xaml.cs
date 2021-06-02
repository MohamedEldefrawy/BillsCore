using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Collections.ObjectModel;
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

        public Orders(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);

            InitializeComponent();
            var company = unitOfWork.Repository<Companies>().GetAll().SingleOrDefault();
            if (company != null)
            {
                lblCompanyName.Content = company.Name.ToString();

            }
            else
            {
                MessageBox.Show("خطأ", "برجاء تسجيل الشركة", MessageBoxButton.OK, MessageBoxImage.Information); ;

            }
        }

        private void BtnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            CreateOrder createOrder = new(_context);
            createOrder.txtUsername.Text = lblUserName.Content.ToString().Split(" ")[1];
            createOrder.txtCompanyName.Text = lblCompanyName.Content.ToString();
            createOrder.Show();
            //Close();
        }

        private void BtnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            ViewOrders viewOrders = new(_context);
            viewOrders.lblUserName.Content = lblUserName.Content.ToString().Split(" ")[1];
            viewOrders.Show();
            //Close();

        }
    }
}
