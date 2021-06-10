using BL.Models;
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
        public Products SelectedProduct { get; set; }

        public UpdateProduct(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            winProducts.ProductsObservalbleCollection.Remove(SelectedProduct);

            SelectedProduct.ID = int.Parse(txtId.Text);
            SelectedProduct.Description = txtDesc.Text;
            SelectedProduct.Name = txtName.Text;
            SelectedProduct.Price = decimal.Parse(txtPrice.Text);

            var result = unitOfWork.Repository<Products>().Update(SelectedProduct);

            if (result < 1)
            {
                MessageBox.Show("فشلت العملية", "فشلت عملية تحديث بيانات العميل", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else
            {
                winProducts.ProductsObservalbleCollection.Add(SelectedProduct);
                MessageBox.Show("تم", "تم تحديث بيانات العميل بنجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}
