using DAL;
using DAL.UnitOfWork;
using System.Collections.Generic;
using System.Windows;
using BL.Models;
using System.Linq;
using System.Windows.Controls;
using DAL.Repositories.Origin;
using System;
using BillsDesktopApp.Dtos.OrdersDtos;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for UpdateOrder.xaml
    /// </summary>
    public partial class UpdateOrder : Window
    {
        private readonly BillsContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Customers> customersRepository;
        private readonly IRepository<BL.Models.Orders> ordersRepository;
        private readonly IRepository<Users> usersRepository;


        public Dictionary<int, string> SelectedCustomer { get; set; }

        public UpdateOrder(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            customersRepository = unitOfWork.Repository<Customers>();
            ordersRepository = unitOfWork.Repository<BL.Models.Orders>();
            usersRepository = unitOfWork.Repository<Users>();
            unitOfWork.Complete();
            InitializeComponent();
            cmbCustomer.SelectedValuePath = "Key";
            cmbCustomer.DisplayMemberPath = "Value";
        }

        private void Load()
        {
            var Choices = GetChoices();
            foreach (var choice in Choices)
            {
                cmbCustomer.Items.Add(choice);
            }

            cmbCustomer.SelectedValue = SelectedCustomer.Keys.First();

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var OldSelectedOrderDto = ViewOrders.orderDTOs
                .Where(o => o.OrderID == int.Parse(txtBillId.Text)).SingleOrDefault();


            var NewSelectedOrderDto = new ShowOrderDTO();
            var selectedOrder = ordersRepository
                .Get(int.Parse(txtBillId.Text));

            UpdateSelectedOrder(selectedOrder);

            ordersRepository.Update(selectedOrder);

            UpdateOrderDto(NewSelectedOrderDto, selectedOrder);

            var result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("فشلت عملية تحديث بيانات الطلب", "فشلت العملية", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                ViewOrders.orderDTOs.Remove(OldSelectedOrderDto);
                ViewOrders.orderDTOs.Add(NewSelectedOrderDto);
                MessageBox.Show("تم تحديث بيانات العميل بنجاح ", "تم", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void UpdateOrderDto(ShowOrderDTO NewSelectedOrderDto, BL.Models.Orders selectedOrder)
        {
            NewSelectedOrderDto.OrderID = selectedOrder.ID;
            NewSelectedOrderDto.Address = selectedOrder.Address;
            NewSelectedOrderDto.CustomerName = cmbCustomer.Text;
            NewSelectedOrderDto.CompanyName = txtCompnayName.Text;
            NewSelectedOrderDto.OrderDate = selectedOrder.Date;
            NewSelectedOrderDto.UserName = txtUserName.Text;
        }

        private void UpdateSelectedOrder(BL.Models.Orders selectedOrder)
        {
            selectedOrder.Address = txtAddress.Text;
            selectedOrder.Date = DateTime.Parse(orderDatepicker.Text);
            selectedOrder.CustomerId = int.Parse(cmbCustomer.SelectedValue.ToString());
        }

        private Dictionary<int, string> GetChoices()
        {
            Dictionary<int, string> choices = new Dictionary<int, string>();

            var Customers = customersRepository.GetAll().ToList();

            foreach (var customer in Customers)
            {
                choices.Add(customer.ID, customer.Name);
            }

            return choices;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void cmbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customerId = cmbCustomer.SelectedValue.ToString();
            var selectedCustomer = customersRepository.Get(int.Parse(customerId));
            unitOfWork.Complete();
            txtAddress.Text = selectedCustomer.Address;
        }
    }
}
