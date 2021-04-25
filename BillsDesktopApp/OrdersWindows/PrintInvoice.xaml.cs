﻿using Bills.DL;
using Bills.DTO;
using BillsDesktopApp.Dtos.OrdersDtos;
using DAL;
using DAL.UnitOfWork;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for PrintInvoice.xaml
    /// </summary>
    public partial class PrintInvoice : Page
    {
        public static ObservableCollection<OrderDto> OrderDetails = new ObservableCollection<OrderDto>();
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;

        public PrintInvoice(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
            dgProducts.ItemsSource = OrderDetails;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(this, "Invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = OrderDetails;

        }
    }
}
