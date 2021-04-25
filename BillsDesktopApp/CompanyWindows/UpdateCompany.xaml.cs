using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Windows;

namespace BillsDesktopApp.CompanyWindows
{
    /// <summary>
    /// Interaction logic for UpdateCompany.xaml
    /// </summary>
    public partial class UpdateCompany : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        public BL.Models.Companies selectedCompany { get; set; }

        private readonly IRepository<Companies> CompaniesService;
        public UpdateCompany(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CompaniesService = unitOfWork.Repository<Companies>();
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            CompanyWindows.Company.CompaniesObservalbleCollection.Remove(selectedCompany);
            selectedCompany.ID = int.Parse(txtId.Text);
            selectedCompany.Name = txtCompanyName.Text;
            CompaniesService.Update(selectedCompany);
            var result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("فشلت العملية", "فشلت عملية تحديث بيانات العميل", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else
            {
                CompanyWindows.Company.CompaniesObservalbleCollection.Add(selectedCompany);
                MessageBox.Show("تم", "تم تحديث بيانات العميل بنجاح ", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
