using BillsDesktopApp.Dtos.OrdersDtos;
using BL.Models;
using DAL;
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

        public CreateOrder(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
            OrderDatepicker.SelectedDateFormat = DatePickerFormat.Short;
            OrderDatepicker.SelectedDate = DateTime.Today;
            cmbCustomerName.SelectedValuePath = "Key";
            cmbCustomerName.DisplayMemberPath = "Value";
            Load();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddOrderDetails addOrderDetails = new AddOrderDetails(_context);
            addOrderDetails.Owner = Window.GetWindow(this);
            addOrderDetails.ShowDialog();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {


        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
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
            var Customers = unitOfWork.Repository<BL.Models.Customers>().GetAll().ToList();
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

        private void dgProducts_MouseEnter(object sender, MouseEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += (item.Price * item.Quantity);
            }

            txtTotalPrice.Text = totalCost.ToString();
        }

        private void dgProducts_MouseLeave(object sender, MouseEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += (item.Price * item.Quantity);
            }

            txtTotalPrice.Text = totalCost.ToString();
        }

        private void dgProducts_CurrentCellChanged(object sender, EventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += (item.Price * item.Quantity);
            }

            txtTotalPrice.Text = totalCost.ToString();
        }

        private void dgProducts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += (item.Price * item.Quantity);
            }

            txtTotalPrice.Text = totalCost.ToString();
        }

        private void dgProducts_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += (item.Price * item.Quantity);
            }

            txtTotalPrice.Text = totalCost.ToString();
        }

        private void btnٍSave_Click(object sender, RoutedEventArgs e)
        {


            var order = new BL.Models.Orders
            {
                CompanyId = unitOfWork.Repository<Companies>().Find(c => c.Name == txtCompanyName.Text).FirstOrDefault().ID,
                CustomerId = int.Parse(cmbCustomerName.SelectedValue.ToString()),
                UserId = unitOfWork.Repository<Users>().Find(u => u.UserName == txtUsername.Text).FirstOrDefault().ID,
                Date = DateTime.Now,
                Address = txtAddress.Text,
            };

            unitOfWork.Repository<BL.Models.Orders>().Add(order);
            var result = unitOfWork.Complete();
            if (result > 0)
            {
                int orderID = order.ID;

                var od = new List<BL.Models.OrderDetails>();

                foreach (var o in OrderDetails)
                {
                    od.Add(new BL.Models.OrderDetails
                    {
                        OrderId = orderID,
                        Price = o.Price,
                        ProductId = o.ProductId,
                        Quantity = o.Quantity,
                    });
                }

                foreach (var item in od)
                {
                    unitOfWork.Repository<OrderDetails>().Add(item);
                    unitOfWork.Complete();
                }

                MessageBox.Show("تم", "تم حفظ الطلب", MessageBoxButton.OK, MessageBoxImage.Information); ;

                var startIndex = cmbCustomerName.SelectedItem.ToString().IndexOf(',');
                var customerName = cmbCustomerName.SelectedItem.ToString().Substring(startIndex + 1);
                customerName = customerName.Remove(customerName.Length - 1);

                PrintInvoice printInvoice = new PrintInvoice(_context);
                printInvoice.OrderDatepicker.Text = OrderDatepicker.Text;
                printInvoice.txtAddress.Text = txtAddress.Text;
                printInvoice.txtCompanyName.Text = txtCompanyName.Text;
                printInvoice.txtInvoiceNumber.Text = orderID.ToString();
                printInvoice.txtUsername.Text = txtUsername.Text;
                printInvoice.txtTotalPrice.Text = txtTotalPrice.Text;
                printInvoice.txtCustomerName.Text = customerName;
                PrintInvoice.OrderDetails = OrderDetails;
                this.Content = printInvoice;
            }

            else
            {
                MessageBox.Show("فشلت العملية", "حدث خطأ أثناء حفظ الطلب", MessageBoxButton.OK, MessageBoxImage.Error); ;

            }

        }
    }
}
