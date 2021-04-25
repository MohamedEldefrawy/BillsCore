using DAL;
using DAL.UnitOfWork;
using System.Windows;

namespace BillsDesktopApp.ProductsWindows
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {

        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;

        public AddProduct(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            unitOfWork.Repository<BL.Models.Products>().Add(new BL.Models.Products
            {
                Name = txtName.Text,
                Description = txtDesc.Text,
                Price = decimal.Parse(txtPrice.Text),
            });

            var result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("خطأ بالتسجيل", "حدث خطأ اثناء إضافة عميل جديد", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("تم", "تم إضافة عميل جديد بنجاح", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
