using BillsDesktopApp.Dtos.OrdersDtos;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for AddOrderDetails.xaml
    /// </summary>
    public partial class AddOrderDetails : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<BL.Models.Products> ProductsService;

        public AddOrderDetails(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            ProductsService = unitOfWork.Repository<BL.Models.Products>();
            InitializeComponent();
            cmbProducts.SelectedValuePath = "Key";
            cmbProducts.DisplayMemberPath = "Value";
            Load();
        }

        private void Load()
        {
            var Choices = GetChoices();
            foreach (var choice in Choices)
            {
                cmbProducts.Items.Add(choice);
            }
        }
        private Dictionary<int, string> GetChoices()
        {
            Dictionary<int, string> choices = new Dictionary<int, string>();
            var Products = ProductsService.GetAll().ToList();
            foreach (var product in Products)
            {
                choices.Add(product.ID, product.Name);
            }
            return choices;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var startIndex = cmbProducts.SelectedItem.ToString().IndexOf(',');
            var productName = cmbProducts.SelectedItem.ToString().Substring(startIndex + 1);
            productName = productName.Remove(productName.Length - 1);

            CreateOrder.OrderDetails.Add(new OrderDto
            {
                ProductId = int.Parse(cmbProducts.SelectedValue.ToString()),
                ProductName = productName,
                Quantity = int.Parse(txtQuantity.Text),
                Price = decimal.Parse(txtPrice.Text),
            });

            this.Close();
        }

        private void cmbProducts_DropDownClosed(object sender, EventArgs e)
        {
            int result;
            var isParsed = int.TryParse(cmbProducts.SelectedValue.ToString(), out result);
            if (isParsed)
            {
                txtPrice.Text = ProductsService.Find(p => p.ID == result)
             .FirstOrDefault().Price.ToString();
            }
        }
    }
}

