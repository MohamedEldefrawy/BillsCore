using BillsDesktopApp.Dtos.OrdersDtos;
using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for ViewOrderDetails.xaml
    /// </summary>
    public partial class ViewOrderDetails : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        public ShowOrderDTO ShowOrderDTO { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }

        public static ObservableCollection<OrderDto> OrderDetailsObservalbleCollection = new ObservableCollection<OrderDto>();

        private readonly IRepository<OrderDetails> OrderDetailsSerivce;
        private readonly IRepository<Products> ProductsServices;


        public ViewOrderDetails(List<OrderDetails> OrderDetails, ShowOrderDTO ShowOrderDTO, BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            OrderDetailsSerivce = unitOfWork.Repository<OrderDetails>();
            ProductsServices = unitOfWork.Repository<Products>();
            this.OrderDetails = OrderDetails;
            this.ShowOrderDTO = ShowOrderDTO;
            InitializeComponent();
        }

        private void Load()
        {
            decimal totalPrice = 0;
            OrderDetailsObservalbleCollection.Clear();

            foreach (var orderDetail in OrderDetails)
            {
                var productName = ProductsServices.Get(orderDetail.ProductId).Name;

                var orderDto = new OrderDto
                {
                    ProductId = orderDetail.ProductId,
                    ProductName = productName,
                    Quantity = orderDetail.Quantity,
                    Price = orderDetail.Price,
                };

                OrderDetailsObservalbleCollection.Add(orderDto);


                totalPrice += orderDetail.Price * orderDetail.Quantity;
            }

            decimal totalCost = 0;

            foreach (var item in OrderDetails)
            {
                totalCost += item.Price * item.Quantity;
            }

            var vat = ((decimal)0.14) * totalCost;
            txtVat.Text = decimal.ToDouble(vat).ToString();
            totalCost += vat;
            txtTotalPrice.Text = decimal.ToDouble(totalCost).ToString(); dgOrderDetails.ItemsSource = OrderDetailsObservalbleCollection;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDto = (OrderDto)dgOrderDetails.SelectedItem;
            var selectedOrderDetail = OrderDetailsSerivce
                .Find(od => od.OrderId == ShowOrderDTO.OrderID)
                .Where(od => od.ProductId == selectedOrderDto.ProductId)
                .FirstOrDefault();
            UpdateOrderDetail updateOrderDetail = new UpdateOrderDetail(_context);
            updateOrderDetail.txtPrice.Text = selectedOrderDto.Price.ToString();
            updateOrderDetail.txtQuantity.Text = selectedOrderDto.Quantity.ToString();
            updateOrderDetail.cmbProducts.Text = selectedOrderDto.ProductName.ToString();
            updateOrderDetail.OrderId = ShowOrderDTO.OrderID;
            updateOrderDetail.OdID = selectedOrderDetail.ID;
            updateOrderDetail.Owner = GetWindow(this);
            updateOrderDetail.ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDto = (OrderDto)dgOrderDetails.SelectedItem;
            var selectedOrderDetail = OrderDetailsSerivce
                .Find(od => od.OrderId == ShowOrderDTO.OrderID)
                .Where(od => od.ProductId == selectedOrderDto.ProductId)
                .FirstOrDefault();

            OrderDetailsObservalbleCollection.Remove(selectedOrderDto);
            OrderDetailsSerivce.Remove(selectedOrderDetail);
            var result = unitOfWork.Complete();
            if (result < 1)
            {
                MessageBox.Show("فشلت العملية", "فشلت عملية مسح سجل من الفاتورة", MessageBoxButton.OK, MessageBoxImage.Error);
                dgOrderDetails.ItemsSource = OrderDetailsObservalbleCollection;
            }
            else
            {
                MessageBox.Show("نجحت العملية", "تم مسح سجل من الفاتورة بنجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                dgOrderDetails.ItemsSource = OrderDetailsObservalbleCollection;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintInvoice printInvoice = new(_context);
            printInvoice.OrderDatepicker.Text = ShowOrderDTO.OrderDate.ToString();
            printInvoice.txtAddress.Text = ShowOrderDTO.Address;
            printInvoice.txtCompanyName.Text = ShowOrderDTO.CompanyName;
            printInvoice.txtVat.Text = txtVat.Text;
            printInvoice.txtInvoiceNumber.Text = ShowOrderDTO.OrderID.ToString();
            printInvoice.txtUsername.Text = ShowOrderDTO.UserName;
            printInvoice.txtTotalPrice.Text = txtTotalPrice.Text;
            printInvoice.txtCustomerName.Text = ShowOrderDTO.CustomerName;
            PrintInvoice.OrderDetails = OrderDetailsObservalbleCollection;
            Content = printInvoice;

        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            RefreshTotalCost();
        }


        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            RefreshTotalCost();
        }

        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            RefreshTotalCost();
        }

        private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            RefreshTotalCost();
        }

        private void RefreshTotalCost()
        {
            decimal totalCost = 0;

            foreach (var item in OrderDetailsObservalbleCollection)
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
