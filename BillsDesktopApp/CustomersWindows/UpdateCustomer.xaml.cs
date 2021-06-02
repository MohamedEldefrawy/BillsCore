using DAL;
using DAL.Repositories.Origin;
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
        private readonly IRepository<BL.Models.Customers> CustomersService;
        public BL.Models.Customers SelectedCustomer { get; set; }
        public UpdateCustomer(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CustomersService = unitOfWork.Repository<BL.Models.Customers>();
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Customers.CustomerObservalbleCollection.Remove(SelectedCustomer);

            SelectedCustomer.ID = int.Parse(txtId.Text);
            SelectedCustomer.Address = txtAddress.Text;
            SelectedCustomer.Email = txtEmail.Text;
            SelectedCustomer.Name = txtName.Text;
            SelectedCustomer.Phone = txtPhone.Text;

            CustomersService.Update(SelectedCustomer);

            var result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("فشلت عملية تحديث بيانات العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else
            {
                Customers.CustomerObservalbleCollection.Add(SelectedCustomer);
                MessageBox.Show("تم تحديث بيانات العميل بنجاح ", "تم", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
        }
    }
}
