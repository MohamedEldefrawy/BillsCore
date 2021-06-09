using BillsDesktopApp.Dtos.OrdersDtos;
using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IRepository<Companies> CompaniesService;
        private readonly IRepository<Customers> CustomersService;
        private readonly IRepository<Users> UsersService;
        private readonly IRepository<OrderDetails> OrderDetailsService;
        public static ObservableCollection<ShowOrderDTO> orderDTOs = new ObservableCollection<ShowOrderDTO>();
        readonly List<string> colNames = new()
        {
            "رقم الفاتورة",
            "اسم المستخدم",
            "اسم الشركة",
            "اسم العميل",
            "التاريخ",
            "العنوان"
        };
        public ViewOrders(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            OrdersServices = unitOfWork.Repository<BL.Models.Orders>();
            CompaniesService = unitOfWork.Repository<Companies>();
            CustomersService = unitOfWork.Repository<Customers>();
            UsersService = unitOfWork.Repository<Users>();
            OrderDetailsService = unitOfWork.Repository<OrderDetails>();
            InitializeComponent();
            cmbSearch.ItemsSource = colNames;
            cmbSearch.SelectedIndex = 0;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDTO = (ShowOrderDTO)dgOrders.SelectedItem;

            Dictionary<int, string> selectedCustomer = new Dictionary<int, string>();
            var customerId = _context.Customers.Where(c => c.Name.ToLower() == selectedOrderDTO.CustomerName.ToLower() &&
            c.Address.ToLower().Trim() == selectedOrderDTO.Address.ToLower().Trim()).FirstOrDefault().ID;

            selectedCustomer.Add(customerId, selectedOrderDTO.CustomerName);
            UpdateOrder updateOrder = new UpdateOrder(_context);
            updateOrder.txtBillId.Text = selectedOrderDTO.OrderID.ToString();
            updateOrder.txtCompnayName.Text = selectedOrderDTO.CompanyName;
            updateOrder.SelectedCustomer = selectedCustomer;
            updateOrder.txtUserName.Text = selectedOrderDTO.UserName;
            updateOrder.orderDatepicker.Text = selectedOrderDTO.OrderDate.ToString();
            updateOrder.txtAddress.Text = selectedOrderDTO.Address;
            updateOrder.Owner = GetWindow(this);
            updateOrder.ShowDialog();
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDTO = (ShowOrderDTO)dgOrders.SelectedItem;
            var orderDetails = OrderDetailsService
                .Find(od => od.OrderId == selectedOrderDTO.OrderID)
                .ToList();

            if (orderDetails.Count < 1)
            {
                MessageBox.Show("حدث خطأ أثناء عرض تفاصيل الفاتورة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ViewOrderDetails viewOrderDetails = new ViewOrderDetails(orderDetails, selectedOrderDTO, _context);
                viewOrderDetails.lblUserName.Content = lblUserName.Content;

                viewOrderDetails.Owner = GetWindow(this);
                viewOrderDetails.ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
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

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
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
            Orders = _context.Orders.Include(o => o.Customer).ToList();

            foreach (var order in Orders)
            {
                var customerName = CustomersService.Find(
                    c => c.ID == order.CustomerId)
                    .FirstOrDefault().Name;

                var selectedUser = UsersService.Find(
                    u => u.ID == order.UserId)
                    .FirstOrDefault();

                var userName = selectedUser.UserName;
                var companyName = CompaniesService.Get(selectedUser.CompanyId).Name;

                if (orderDTOs.Count == Orders.Count)
                {
                    orderDTOs.Clear();
                }

                orderDTOs.Add(new ShowOrderDTO
                {
                    Address = order.Customer.Address,
                    CustomerName = customerName,
                    UserName = userName,
                    OrderID = order.ID,
                    OrderDate = order.Date,
                    CompanyName = companyName

                });
            }

            dgOrders.ItemsSource = orderDTOs;
        }

        private List<ShowOrderDTO> GetSelectedFilter()
        {
            List<ShowOrderDTO> orders;
            switch (cmbSearch.SelectedIndex)
            {
                case 0:
                    {
                        orders = orderDTOs.Where(o => o.OrderID.ToString().StartsWith(txtSearch.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }

                case 1:
                    {
                        orders = orderDTOs.Where(o => o.UserName.ToString().StartsWith(txtSearch.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }

                case 2:
                    {
                        orders = orderDTOs.Where(o => o.CompanyName.ToString().StartsWith(txtSearch.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }

                case 3:
                    {
                        orders = orderDTOs.Where(o => o.CustomerName.ToString().StartsWith(txtSearch.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                case 4:
                    {
                        orders = orderDTOs.Where(o => o.OrderDate.ToString().StartsWith(txtSearch.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                default:
                    {
                        orders = orderDTOs.Where(o => o.Address.ToString().StartsWith(txtSearch.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
            }

            return orders;
        }

    }
}
