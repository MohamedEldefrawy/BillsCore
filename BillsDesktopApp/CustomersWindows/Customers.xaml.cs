using DAL;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BillsDesktopApp.CustomersWindows
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Window
    {
        private static BillsContext _context;
        private readonly UnitOfWork unitOfWork;
        public static DataGrid DataGrid;
        private string[] AllcolNames = typeof(BL.Models.Customers).GetProperties().Select(p => p.Name).ToArray();
        private List<string> selectedColNames = new List<string>();

        public Customers(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            foreach (var col in AllcolNames)
            {
                if (col != "Orders")
                {
                    selectedColNames.Add(col);

                }
            }
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            cmbSearch.ItemsSource = selectedColNames;
            cmbSearch.SelectedIndex = 0;
            dgCustomers.ItemsSource = unitOfWork.Repository<BL.Models.Customers>().GetAll().ToList();
            DataGrid = dgCustomers;

        }
        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var customers = GetSelectedFilter();
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgCustomers.ItemsSource = unitOfWork.Repository<BL.Models.Customers>().GetAll().ToArray();

            }
            dgCustomers.ItemsSource = customers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer(_context);
            addCustomer.Owner = Window.GetWindow(this);
            addCustomer.ShowDialog();

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = (BL.Models.Customers)dgCustomers.SelectedItem;
            UpdateCustomer updateCustomer = new UpdateCustomer(_context);
            updateCustomer.txtName.Text = selectedCustomer.Name;
            updateCustomer.txtAddress.Text = selectedCustomer.Address;
            updateCustomer.txtEmail.Text = selectedCustomer.Email;
            updateCustomer.txtPhone.Text = selectedCustomer.Phone;
            updateCustomer.txtId.Text = selectedCustomer.ID.ToString();
            updateCustomer.Owner = Window.GetWindow(this);
            updateCustomer.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = (BL.Models.Customers)dgCustomers.SelectedItem;
            unitOfWork.Repository<BL.Models.Customers>().Remove(selectedCustomer);
            var result = unitOfWork.Complete();
            if (result < 1)
            {
                MessageBox.Show("فشلت عملية مسح العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                MessageBox.Show("تم مسح العميل بنجاح", "نجحت العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                dgCustomers.ItemsSource = unitOfWork.Repository<BL.Models.Customers>().GetAll().ToArray();
            }
        }

        private List<BL.Models.Customers> GetSelectedFilter()
        {
            List<BL.Models.Customers> customers;
            switch (cmbSearch.SelectedIndex)
            {
                case 0:
                    {
                        customers = unitOfWork.Repository<BL.Models.Customers>().Find(c => c.Name.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 1:
                    {
                        customers = unitOfWork.Repository<BL.Models.Customers>().Find(c => c.Phone.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 2:
                    {
                        customers = unitOfWork.Repository<BL.Models.Customers>().Find(c => c.Address.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                default:
                    {
                        customers = unitOfWork.Repository<BL.Models.Customers>().Find(c => c.Email.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }
            }

            return customers;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgCustomers.ItemsSource = unitOfWork.Repository<BL.Models.Customers>().GetAll().ToArray();
        }
    }
}
