using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BillsDesktopApp.CompanyWindows
{
    /// <summary>
    /// Interaction logic for Company.xaml
    /// </summary>
    public partial class Company : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<BL.Models.Companies> CompaniesService;
        private string[] AllcolNames = typeof(BL.Models.Companies).GetProperties().Select(p => p.Name).ToArray();
        private List<string> selectedColNames = new List<string>();
        public static ObservableCollection<BL.Models.Companies> CompaniesObservalbleCollection = new ObservableCollection<BL.Models.Companies>();
        public Company(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CompaniesService = unitOfWork.Repository<BL.Models.Companies>();
            InitializeComponent();
            foreach (var col in AllcolNames)
            {
                if (col != "Orders")
                {
                    selectedColNames.Add(col);

                }
            }
            Load();
        }

        private void btnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            RegisterCompany registerCompany = new RegisterCompany(_context);
            registerCompany.lblUserName.Content = lblUserName.Content.ToString().Split(" ")[0];
            registerCompany.Owner = GetWindow(this);
            registerCompany.ShowDialog();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompany = (Companies)dgCompanies.SelectedItem;
            UpdateCompany updateCompany = new UpdateCompany(_context);
            updateCompany.lblUserName.Content = lblUserName.Content.ToString().Split(" ")[0];
            updateCompany.txtCompanyName.Text = selectedCompany.Name;
            updateCompany.txtId.Text = selectedCompany.ID.ToString();
            updateCompany.selectedCompany = selectedCompany;
            updateCompany.Owner = GetWindow(this);
            updateCompany.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompany = (Companies)dgCompanies.SelectedItem;

            CompaniesService.Remove(selectedCompany);
            CompaniesObservalbleCollection.Remove(selectedCompany);
            var result = unitOfWork.Complete();
            if (result < 1)
            {
                MessageBox.Show("فشلت عملية مسح العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                MessageBox.Show("تم مسح العميل بنجاح", "نجحت العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                dgCompanies.ItemsSource = CompaniesObservalbleCollection;
            }
        }
        private void Load()
        {
            var companies = CompaniesService.GetAll().ToList();
            foreach (var company in companies)
            {
                CompaniesObservalbleCollection.Add(company);
            }

            dgCompanies.ItemsSource = CompaniesObservalbleCollection;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            dgCompanies.ItemsSource = CompaniesObservalbleCollection;
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            dgCompanies.ItemsSource = CompaniesObservalbleCollection;

        }
    }
}
