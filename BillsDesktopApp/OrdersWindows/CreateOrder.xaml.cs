using BillsDesktopApp.Dtos.OrdersDtos;
using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class CreateOrder : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;

        public static ObservableCollection<OrderDto> OrderDetails = new ObservableCollection<OrderDto>();

        private readonly IRepository<BL.Models.Orders> OrdersService;
        private readonly IRepository<OrderDetails> OrderDetailsService;

        public CreateOrder(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            OrdersService = unitOfWork.Repository<BL.Models.Orders>();
            OrderDetailsService = unitOfWork.Repository<OrderDetails>();
            InitializeComponent();
            OrderDatepicker.SelectedDateFormat = DatePickerFormat.Short;
            OrderDatepicker.SelectedDate = DateTime.Today;
            cmbCustomerName.SelectedValuePath = "Key";
            cmbCustomerName.DisplayMemberPath = "Value";
            Load();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddOrderDetails addOrderDetails = new(_context);
            addOrderDetails.Owner = GetWindow(this);
            addOrderDetails.ShowDialog();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {


        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            OrderDetails.RemoveAt(dgProducts.SelectedIndex);
        }

        private void Load()
        {
            var Choices = GetChoices();
            foreach (var choice in Choices)
            {
                cmbCustomerName.Items.Add(choice);
            }

            dgProducts.ItemsSource = OrderDetails;
        }

        private Dictionary<int, string> GetChoices()
        {
            Dictionary<int, string> choices = new Dictionary<int, string>();
            var Customers = unitOfWork.Repository<Customers>().GetAll().ToList();

            foreach (var customer in Customers)
            {
                choices.Add(customer.ID, customer.Name);
            }

            return choices;
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = OrderDetails;
        }

        private void DgProducts_MouseEnter(object sender, MouseEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }

            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }

        private void DgProducts_MouseLeave(object sender, MouseEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }
            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }

        private void DgProducts_CurrentCellChanged(object sender, EventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }

            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }

        private void DgProducts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }
            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }

        private void DgProducts_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }

            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var orderDetails = new List<OrderDetails>();

            foreach (var o in OrderDetails)
            {
                orderDetails.Add(new OrderDetails
                {
                    Price = o.Price,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                });
            }

            var order = new BL.Models.Orders
            {
                CustomerId = int.Parse(cmbCustomerName.SelectedValue.ToString()),
                UserId = unitOfWork.Repository<Users>().Find(u => u.UserName == txtUsername.Text).FirstOrDefault().ID,
                Date = DateTime.Now,
                Address = txtAddress.Text,
                OrderDetails = orderDetails
            };

            var result = OrdersService.Add(order);

            if (result > 0)
            {
                var orderId = order.ID;
                MessageBox.Show("تم", "تم حفظ الطلب", MessageBoxButton.OK, MessageBoxImage.Information); ;
                var startIndex = cmbCustomerName.SelectedItem.ToString().IndexOf(',');
                var customerName = cmbCustomerName.SelectedItem.ToString().Substring(startIndex + 1);
                customerName = customerName.Remove(customerName.Length - 1);

                PrintInvoice printInvoice = new PrintInvoice(_context);
                printInvoice.OrderDatepicker.Text = OrderDatepicker.Text;
                printInvoice.txtAddress.Text = txtAddress.Text;
                printInvoice.txtCompanyName.Text = txtCompanyName.Text;
                printInvoice.txtInvoiceNumber.Text = orderId.ToString();
                printInvoice.txtUsername.Text = txtUsername.Text;
                printInvoice.txtTotalPrice.Text = txtTotalPrice.Text;
                printInvoice.txtCustomerName.Text = customerName;
                PrintInvoice.OrderDetails = OrderDetails;
                Content = printInvoice;
            }
            else
            {
                MessageBox.Show("فشلت العملية", "حدث خطأ أثناء حفظ الطلب", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CmbCustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customerId = cmbCustomerName.SelectedValue;
            var selctedCustomerAddress = _context.Customers.Find(customerId).Address;
            txtAddress.Text = selctedCustomerAddress;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }

            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }
            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }
            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString();
        }
    }
}
