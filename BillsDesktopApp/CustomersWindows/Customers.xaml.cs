using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static ObservableCollection<BL.Models.Customers> CustomerObservalbleCollection = new ObservableCollection<BL.Models.Customers>();

        private readonly string[] AllcolNames = typeof(BL.Models.Customers).GetProperties().Select(p => p.Name).ToArray();
        private readonly IRepository<BL.Models.Customers> CustomersService;
        private List<string> selectedColNames = new List<string>();

        public Customers(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CustomersService = unitOfWork.Repository<BL.Models.Customers>();

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
            var customers = CustomersService.GetAll().ToList();

            foreach (var customer in customers)
            {
                CustomerObservalbleCollection.Add(customer);
            }

            dgCustomers.ItemsSource = customers;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var customers = GetSelectedFilter();

            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgCustomers.ItemsSource = CustomersService.GetAll().ToArray();
            }

            dgCustomers.ItemsSource = customers;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomer = new(_context);
            addCustomer.Owner = GetWindow(this);
            addCustomer.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = (BL.Models.Customers)dgCustomers.SelectedItem;
            UpdateCustomer updateCustomer = new UpdateCustomer(_context);
            updateCustomer.txtName.Text = selectedCustomer.Name;
            updateCustomer.txtAddress.Text = selectedCustomer.Address;
            updateCustomer.txtEmail.Text = selectedCustomer.Email;
            updateCustomer.txtPhone.Text = selectedCustomer.Phone;
            updateCustomer.txtId.Text = selectedCustomer.ID.ToString();
            updateCustomer.SelectedCustomer = selectedCustomer;
            updateCustomer.Owner = GetWindow(this);
            updateCustomer.ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = (BL.Models.Customers)dgCustomers.SelectedItem;

            var result = CustomersService.Remove(selectedCustomer);

            if (result < 1)
            {
                MessageBox.Show("فشلت عملية مسح العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("تم مسح العميل بنجاح", "نجحت العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                CustomerObservalbleCollection.Remove(selectedCustomer);
            }


            dgCustomers.ItemsSource = CustomerObservalbleCollection;
        }

        private List<BL.Models.Customers> GetSelectedFilter()
        {
            List<BL.Models.Customers> customers;

            switch (cmbSearch.SelectedIndex)
            {
                case 0:
                    {
                        customers = CustomersService.Find(c => c.Name.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 1:
                    {
                        customers = CustomersService.Find(c => c.Phone.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 2:
                    {
                        customers = CustomersService.Find(c => c.Address.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                default:
                    {
                        customers = CustomersService.Find(c => c.Email.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }
            }

            return customers;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            dgCustomers.ItemsSource = CustomerObservalbleCollection;
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            dgCustomers.ItemsSource = CustomerObservalbleCollection;
        }
    }
}

