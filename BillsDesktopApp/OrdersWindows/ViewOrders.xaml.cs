using BillsDesktopApp.Dtos.OrdersDtos;
using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for ViewOrders.xaml
    /// </summary>
    public partial class ViewOrders : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        List<BL.Models.Orders> Orders = new List<BL.Models.Orders>();
        private readonly IRepository<BL.Models.Orders> OrdersServices;
        private readonly IRepository<BL.Models.Companies> CompaniesService;
        private readonly IRepository<BL.Models.Customers> CustomersService;
        private readonly IRepository<BL.Models.Users> UsersService;
        private readonly IRepository<BL.Models.OrderDetails> OrderDetailsService;
        List<ShowOrderDTO> orderDTOs = new List<ShowOrderDTO>();
        List<string> colNames = new List<string> { "رقم الفاتورة","اسم المستخدم"
        ,"اسم الشركة","اسم العميل","التاريخ","العنوان"};
        public ViewOrders(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            OrdersServices = unitOfWork.Repository<BL.Models.Orders>();
            CompaniesService = unitOfWork.Repository<BL.Models.Companies>();
            CustomersService = unitOfWork.Repository<BL.Models.Customers>();
            UsersService = unitOfWork.Repository<BL.Models.Users>();
            OrderDetailsService = unitOfWork.Repository<BL.Models.OrderDetails>();
            InitializeComponent();
            cmbSearch.ItemsSource = colNames;
            cmbSearch.SelectedIndex = 0;
        }

        private void Load()
        {
            dgOrders.ItemsSource = orderDTOs;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDTO = (ShowOrderDTO)dgOrders.SelectedItem;
            UpdateOrder updateOrder = new UpdateOrder(_context);
            updateOrder.txtBillId.Text = selectedOrderDTO.OrderID.ToString();
            updateOrder.txtCompnayName.Text = selectedOrderDTO.CompanyName;
            updateOrder.txtCustomerName.Text = selectedOrderDTO.CustomerName;
            updateOrder.txtUserName.Text = selectedOrderDTO.UserName;
            updateOrder.orderDatepicker.Text = selectedOrderDTO.OrderDate.ToString();
            updateOrder.txtAddress.Text = selectedOrderDTO.Address;
            updateOrder.Owner = Window.GetWindow(this);
            updateOrder.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDTO = (ShowOrderDTO)dgOrders.SelectedItem;
            var orderDetails = OrderDetailsService
                .Find(od => od.OrderId == selectedOrderDTO.OrderID)
                .ToList();

            ViewOrderDetails viewOrderDetails = new ViewOrderDetails(orderDetails, selectedOrderDTO, _context);
            viewOrderDetails.lblUserName.Content = lblUserName.Content;

            viewOrderDetails.Owner = Window.GetWindow(this);
            viewOrderDetails.ShowDialog();

        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDto = (ShowOrderDTO)dgOrders.SelectedItem;
            var selectedOrder = OrdersServices.Find(o => o.ID == selectedOrderDto.OrderID).FirstOrDefault();

            orderDTOs.Remove(selectedOrderDto);
            OrdersServices.Remove(selectedOrder);
            var result = unitOfWork.Complete();
            if (result < 1)
            {
                MessageBox.Show("فشلت عملية مسح الفاتورة", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);
                dgOrders.ItemsSource = orderDTOs;
            }
            else
            {
                MessageBox.Show("تم مسح الفاتورة بنجاح", "نجحت العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                dgOrders.ItemsSource = orderDTOs;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var orders = GetSelectedFilter();
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgOrders.ItemsSource = orderDTOs;

            }
            dgOrders.ItemsSource = orders;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Orders = OrdersServices.GetAll().ToList();

            foreach (var order in Orders)
            {
                var companyName = CompaniesService.Find(
                    c => c.ID == order.CompanyId)
                    .FirstOrDefault().Name;

                var customerName = CustomersService.Find(
                    c => c.ID == order.CustomerId)
                    .FirstOrDefault().Name;

                var userName = UsersService.Find(
                    u => u.ID == order.UserId)
                    .FirstOrDefault().UserName;


                orderDTOs.Add(new ShowOrderDTO
                {
                    Address = order.Address,
                    CompanyName = companyName,
                    CustomerName = customerName,
                    UserName = userName,
                    OrderID = order.ID,
                    OrderDate = order.Date
                });
            }

            Load();
        }

        private List<ShowOrderDTO> GetSelectedFilter()
        {
            List<ShowOrderDTO> orders;
            switch (cmbSearch.SelectedIndex)
            {
                case 0:
                    {
                        orders = orderDTOs.Where(o => o.OrderID.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 1:
                    {
                        orders = orderDTOs.Where(o => o.UserName.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 2:
                    {
                        orders = orderDTOs.Where(o => o.CompanyName.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }

                case 3:
                    {
                        orders = orderDTOs.Where(o => o.CustomerName.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }
                case 4:
                    {
                        orders = orderDTOs.Where(o => o.OrderDate.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }
                default:
                    {
                        orders = orderDTOs.Where(o => o.Address.ToString().StartsWith(txtSearch.Text)).ToList();
                        break;
                    }
            }

            return orders;
        }

    }
}
