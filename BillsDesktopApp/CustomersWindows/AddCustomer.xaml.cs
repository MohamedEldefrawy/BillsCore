using DAL;
using DAL.UnitOfWork;
using System.Windows;

namespace BillsDesktopApp.CustomersWindows
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        public AddCustomer(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var customer = new BL.Models.Customers
            {
                Name = txtName.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                Email = txtEmail.Text
            };

            unitOfWork.Repository<BL.Models.Customers>().Add(customer);

            var result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("خطأ بالتسجيل", "حدث خطأ اثناء إضافة عميل جديد", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Customers.CustomerObservalbleCollection.Add(customer);
                MessageBox.Show("تم", "تم إضافة عميل جديد بنجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
        }
    }
}
