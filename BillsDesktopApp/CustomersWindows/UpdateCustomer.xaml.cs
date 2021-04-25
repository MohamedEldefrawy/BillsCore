using DAL;
using DAL.UnitOfWork;
using System.Windows;

namespace BillsDesktopApp.CustomersWindows
{
    /// <summary>
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        public UpdateCustomer(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            unitOfWork.Repository<BL.Models.Customers>().Update(new BL.Models.Customers
            {
                ID = int.Parse(txtId.Text),
                Address = txtAddress.Text,
                Email = txtEmail.Text,
                Name = txtName.Text,
                Phone = txtName.Text,
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
