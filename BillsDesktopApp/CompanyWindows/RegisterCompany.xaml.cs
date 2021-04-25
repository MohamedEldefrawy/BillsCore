using BL.Models;
using DAL;
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
        public RegisterCompany(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            var companies = unitOfWork.Repository<Companies>().GetAll().ToList();

            if (companies.Count == 0)
            {
                unitOfWork.Repository<Companies>().Add(new BL.Models.Companies
                {
                    Name = txtCompanyName.Text
                });

                var result = unitOfWork.Complete();
                if (result < 0)
                {
                    MessageBox.Show("حدث خطأ اثناء تسجيل الشركة", "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Error); ;

                }
                else
                {
                    MessageBox.Show("تم تسجيل الشركة بالنجاح", "تم", MessageBoxButton.OK, MessageBoxImage.Information); ;

                }
            }
            else
            {
                MessageBox.Show("تم التسجيل من قبل", "خطأ",
                     MessageBoxButton.OK, MessageBoxImage.Error); ;
            }
        }
    }
}
