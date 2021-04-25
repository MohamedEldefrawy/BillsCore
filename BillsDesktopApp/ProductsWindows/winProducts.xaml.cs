using DAL;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BillsDesktopApp.ProductsWindows
{
    /// <summary>
    /// Interaction logic for winProducts.xaml
    /// </summary>
    public partial class winProducts : Window
    {

        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        public static DataGrid DataGrid;
        private string[] AllcolNames = typeof(BL.Models.Products).GetProperties().Select(p => p.Name).ToArray();
        private List<string> selectedColNames = new List<string>();
        public winProducts(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            foreach (var col in AllcolNames)
            {
                if (col != "OrderDetails")
                {
                    selectedColNames.Add(col);

                }
            }
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            cmbSearch.ItemsSource = selectedColNames;
            cmbSearch.SelectedIndex = 0;
            dgProducts.ItemsSource = unitOfWork.Repository<BL.Models.Products>().GetAll().ToList();
            DataGrid = dgProducts;

        }

        private List<BL.Models.Products> GetSelectedFilter()
        {
            List<BL.Models.Products> products;
            switch (cmbSearch.SelectedIndex)
            {
                case 0:
                    {
                        products = unitOfWork.Repository<BL.Models.Products>().Find(c => c.Name.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 1:
                    {
                        products = unitOfWork.Repository<BL.Models.Products>().Find(c => c.Description.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                default:
                    {
                        products = unitOfWork.Repository<BL.Models.Products>().Find(c => c.Price.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }
            }

            return products;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var products = GetSelectedFilter();
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgProducts.ItemsSource = unitOfWork.Repository<BL.Models.Customers>().GetAll().ToArray();

            }
            dgProducts.ItemsSource = products;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addProduct = new AddProduct(_context);
            addProduct.Owner = Window.GetWindow(this);
            addProduct.ShowDialog();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = unitOfWork.Repository<BL.Models.Products>().GetAll().ToArray();

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (BL.Models.Products)dgProducts.SelectedItem;
            UpdateProduct updateProduct = new UpdateProduct(_context);
            updateProduct.txtId.Text = selectedProduct.ID.ToString();
            updateProduct.txtName.Text = selectedProduct.Name;
            updateProduct.txtDesc.Text = selectedProduct.Description;
            updateProduct.txtPrice.Text = selectedProduct.Price.ToString();
            updateProduct.Owner = Window.GetWindow(this);
            updateProduct.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (BL.Models.Products)dgProducts.SelectedItem;
            unitOfWork.Repository<BL.Models.Products>().Remove(selectedProduct);
            var result = unitOfWork.Complete();
            if (result < 1)
            {
                MessageBox.Show("فشلت عملية مسح العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                MessageBox.Show("تم مسح العميل بنجاح", "نجحت العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                dgProducts.ItemsSource = unitOfWork.Repository<BL.Models.Customers>().GetAll().ToArray();
            }
        }
    }
}
