using DAL;
using DAL.UnitOfWork;
using System.Windows;

namespace BillsDesktopApp.ProductsWindows
{
    /// <summary>
    /// Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {

        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        public UpdateProduct(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            unitOfWork.Repository<BL.Models.Products>().Update(new BL.Models.Products
            {
                ID = int.Parse(txtId.Text),
                Name = txtName.Text,
                Description = txtDesc.Text,
                Price = decimal.Parse(txtPrice.Text),
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
