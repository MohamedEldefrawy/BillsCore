using DAL;
using DAL.Repositories.Origin;
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
        private readonly IRepository<BL.Models.Customers> CustomersRepository;
        public AddCustomer(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CustomersRepository = unitOfWork.Repository<BL.Models.Customers>();
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

            var result = CustomersRepository.Add(customer);

            if (result < 1)
            {
                MessageBox.Show("حدث خطأ اثناء إضافة عميل جديد", "خطأ بالتسجيل", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Customers.CustomerObservalbleCollection.Add(customer);
                MessageBox.Show("تم إضافة عميل جديد بنجاح", "تم", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();

            }
        }
    }
}
