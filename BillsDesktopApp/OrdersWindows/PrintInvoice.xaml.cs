using BillsDesktopApp.Dtos.OrdersDtos;
using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BillsDesktopApp.OrdersWindows
{
    /// <summary>
    /// Interaction logic for PrintInvoice.xaml
    /// </summary>
    public partial class PrintInvoice : Page
    {
        public static ObservableCollection<OrderDto> OrderDetails = new();
        private readonly IRepository<Companies> companiesRepository;
        private readonly IRepository<Users> usersRepository;
        private readonly UnitOfWork _unitOfWork;

        public PrintInvoice(BillsContext context)
        {
            _unitOfWork = new UnitOfWork(context);
            companiesRepository = _unitOfWork.Repository<Companies>();
            usersRepository = _unitOfWork.Repository<Users>();
            InitializeComponent();
            dgProducts.ItemsSource = OrderDetails;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(this, "Invoice");
                }
            }

            finally
            {
                Visibility = Visibility.Hidden;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            (Parent as Window).WindowState = WindowState.Maximized;

            var selectedUser = usersRepository.Find(u => u.UserName == txtUsername.Text).SingleOrDefault();
            var selectedCompany = companiesRepository.Find(c => c.ID == selectedUser.CompanyId).SingleOrDefault();


            string destenationPathLogo = Environment.CurrentDirectory + @"\Static\Logos\" + selectedCompany.LogoImagePath;
            BitmapImage bitmapLogo = new();
            bitmapLogo.BeginInit();
            bitmapLogo.UriSource = new Uri(destenationPathLogo);
            bitmapLogo.EndInit();
            imgLogo.Source = bitmapLogo;


            string destenationPathSignutre = Environment.CurrentDirectory + @"\Static\Signutres\" + selectedCompany.SignutreImagePath;
            BitmapImage bitmapSignutre = new();
            bitmapSignutre.BeginInit();
            bitmapSignutre.UriSource = new Uri(destenationPathSignutre);
            bitmapSignutre.EndInit();
            imgSignutre.Source = bitmapSignutre;


            dgProducts.ItemsSource = OrderDetails;
        }
    }
}
