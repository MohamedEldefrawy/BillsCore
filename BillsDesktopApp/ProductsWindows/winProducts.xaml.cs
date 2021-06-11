using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string[] AllcolNames = typeof(BL.Models.Products).GetProperties().Select(p => p.Name).ToArray();
        public static ObservableCollection<BL.Models.Products> ProductsObservalbleCollection = new();
        private readonly IRepository<BL.Models.Products> ProductsService;
        private readonly List<string> selectedColNames = new();

        public winProducts(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            ProductsService = unitOfWork.Repository<BL.Models.Products>();

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

            var products = ProductsService.GetAll().ToList();
            ProductsObservalbleCollection.Clear();

            foreach (var product in products)
            {
                ProductsObservalbleCollection.Add(product);
            }

            dgProducts.ItemsSource = ProductsObservalbleCollection;
        }

        private List<BL.Models.Products> GetSelectedFilter()
        {
            List<BL.Models.Products> products;
            switch (cmbSearch.SelectedIndex)
            {
                case 0:
                    {
                        products = ProductsService.Find(c => c.Name.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 1:
                    {
                        products = ProductsService.Find(c => c.Description.StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                default:
                    {
                        products = ProductsService.Find(c => c.Price.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }
            }

            return products;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var products = GetSelectedFilter();

            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgProducts.ItemsSource = ProductsObservalbleCollection;

            }
            else
            {
                ProductsObservalbleCollection.Clear();

                foreach (var product in products)
                {
                    ProductsObservalbleCollection.Add(product);
                }

                dgProducts.ItemsSource = products;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addProduct = new AddProduct(_context)
            {
                Owner = GetWindow(this)
            };

            addProduct.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (BL.Models.Products)dgProducts.SelectedItem;

            UpdateProduct updateProduct = new(_context);
            updateProduct.txtId.Text = selectedProduct.ID.ToString();
            updateProduct.txtName.Text = selectedProduct.Name;
            updateProduct.txtDesc.Text = selectedProduct.Description;
            updateProduct.txtPrice.Text = selectedProduct.Price.ToString();
            updateProduct.SelectedProduct = selectedProduct;
            updateProduct.Owner = GetWindow(this);
            updateProduct.ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (BL.Models.Products)dgProducts.SelectedItem;

            var result = ProductsService.Remove(selectedProduct);

            if (result < 1)
            {
                MessageBox.Show("فشلت العملية", "فشلت عملية مسح العميل", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                ProductsObservalbleCollection.Remove(selectedProduct);
                MessageBox.Show("نجحت العملية", "تم مسح العميل بنجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                dgProducts.ItemsSource = ProductsObservalbleCollection;
            }
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = ProductsObservalbleCollection;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = ProductsObservalbleCollection;

        }
    }
}
