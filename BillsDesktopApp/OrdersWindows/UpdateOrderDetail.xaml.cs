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
    /// Interaction logic for UpdateOrderDetail.xaml
    /// </summary>
    public partial class UpdateOrderDetail : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        //private readonly IRepository<BL.Models.OrderDetails> OrderDetailsService;
        private readonly IRepository<BL.Models.Products> ProductsService;
        public int OrderId { get; set; }
        public int OdID { get; set; }

        public UpdateOrderDetail(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            ProductsService = unitOfWork.Repository<BL.Models.Products>();
            //OrderDetailsService = unitOfWork.Repository<BL.Models.OrderDetails>();
            InitializeComponent();
            cmbProducts.SelectedValuePath = "Key";
            cmbProducts.DisplayMemberPath = "Value";
            Load();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            IRepository<BL.Models.OrderDetails> OrderDetailsService =
                unitOfWork.Repository<BL.Models.OrderDetails>();

            var startIndex = cmbProducts.SelectedItem.ToString().IndexOf(',');
            var productName = cmbProducts.SelectedItem.ToString().Substring(startIndex + 1);
            productName = productName.Remove(productName.Length - 1);

            BL.Models.OrderDetails od = new BL.Models.OrderDetails
            {
                ID = OdID,
                ProductId = int.Parse(cmbProducts.SelectedValue.ToString()),
                Price = decimal.Parse(txtPrice.Text),
                Quantity = int.Parse(txtQuantity.Text),
                OrderId = OrderId,
            };


            var result = OrderDetailsService.Update(od);


            if (result < 1)
            {
                MessageBox.Show("فشلت عملية تحديث بيانات العميل", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);


            }

            else
            {
                MessageBox.Show("تم تحديث بيانات العميل بنجاح ", "تم", MessageBoxButton.OK, MessageBoxImage.Information);

                var orderDetails = OrderDetailsService
                    .Find(od => od.OrderId == OrderId).ToList();

                ViewOrderDetails.OrderDetailsObservalbleCollection.Clear();

                foreach (var orderdetail in orderDetails)
                {
                    var odDto = new OrderDto
                    {
                        ProductId = orderdetail.ProductId,
                        ProductName = ProductsService.Find(p => p.ID == orderdetail.ProductId)
                        .FirstOrDefault().Name,
                        Quantity = orderdetail.Quantity,
                        Price = orderdetail.Price,
                    };

                    ViewOrderDetails.OrderDetailsObservalbleCollection.Add(odDto);
                }
            }

            this.Close();
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
