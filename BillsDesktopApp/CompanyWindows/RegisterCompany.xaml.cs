using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Linq;
using System.Windows;

namespace BillsDesktopApp.CompanyWindows
{
    /// <summary>
    /// Interaction logic for RegisterCompany.xaml
    /// </summary>
    public partial class RegisterCompany : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Companies> CompaniesService;
        public RegisterCompany(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CompaniesService = unitOfWork.Repository<Companies>();
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var companies = CompaniesService.GetAll().ToList();


            if (companies.Count == 0)
            {
                var company = new BL.Models.Companies
                {
                    Name = txtCompanyName.Text
                };

                CompaniesService.Add(company);

                var result = unitOfWork.Complete();
                if (result < 0)
                {
                    MessageBox.Show("حدث خطأ اثناء تسجيل الشركة", "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Error); ;
                }
                else
                {
                    MessageBox.Show("تم تسجيل الشركة بالنجاح", "تم", MessageBoxButton.OK, MessageBoxImage.Information);
                    BillsDesktopApp.CompanyWindows.Company.CompaniesObservalbleCollection.Add(company);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("تم التسجيل من قبل", "خطأ",
                     MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
