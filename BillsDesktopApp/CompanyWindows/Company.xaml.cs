using BL.Models;
using DAL;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BillsDesktopApp.CompanyWindows
{
    /// <summary>
    /// Interaction logic for Company.xaml
    /// </summary>
    public partial class Company : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        private string[] AllcolNames = typeof(BL.Models.Companies).GetProperties().Select(p => p.Name).ToArray();
        private List<string> selectedColNames = new List<string>();
        public static DataGrid DataGrid;
        public Company(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
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
            registerCompany.Owner = Window.GetWindow(this);
            registerCompany.ShowDialog();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompany = (BL.Models.Companies)dgCompanies.SelectedItem;
            UpdateCompany updateCompany = new UpdateCompany(_context);
            updateCompany.lblUserName.Content = lblUserName.Content.ToString().Split(" ")[0];
            updateCompany.txtCompanyName.Text = selectedCompany.Name;
            updateCompany.txtId.Text = selectedCompany.ID.ToString();
            updateCompany.Owner = Window.GetWindow(this);
            updateCompany.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompany = (BL.Models.Companies)dgCompanies.SelectedItem;

            unitOfWork.Repository<Companies>().Remove(selectedCompany);
            var result = unitOfWork.Complete();
            if (result < 1)
            {
                MessageBox.Show("فشلت عملية مسح العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                MessageBox.Show("تم مسح العميل بنجاح", "نجحت العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                dgCompanies.ItemsSource = unitOfWork.Repository<Companies>().GetAll().ToArray();
            }
        }
        private void Load()
        {
            dgCompanies.ItemsSource = unitOfWork.Repository<Companies>().GetAll().ToList();
            DataGrid = dgCompanies;

        }
    }
}
