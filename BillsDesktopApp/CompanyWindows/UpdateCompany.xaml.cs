using BL.Models;
using DAL;
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
        public UpdateCompany(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            unitOfWork.Repository<Companies>().Update(new BL.Models.Companies
            {
                ID = int.Parse(txtId.Text),

                Name = txtCompanyName.Text,
            });

            var result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("فشلت عملية تحديث بيانات العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else
            {
                MessageBox.Show("تم تحديث بيانات العميل بنجاح ", "تم", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
