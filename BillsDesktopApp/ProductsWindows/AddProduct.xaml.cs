using BL.Models;
using DAL;
using DAL.Repositories.Origin;
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

        private readonly IRepository<Products> ProductsService;
        public AddProduct(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            ProductsService = unitOfWork.Repository<Products>();
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var product = new BL.Models.Products
            {
                Name = txtName.Text,
                Description = txtDesc.Text,
                Price = decimal.Parse(txtPrice.Text),
            };

            ProductsService.Add(product);

            var result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("خطأ بالتسجيل", "حدث خطأ اثناء إضافة عميل جديد", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("تم", "تم إضافة عميل جديد بنجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                winProducts.ProductsObservalbleCollection.Add(product);
                this.Close();

            }
        }
    }
}
