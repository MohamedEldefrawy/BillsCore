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
        public static ObservableCollection<OrderDto> OrderDetails = new ObservableCollection<OrderDto>();
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
                IsEnabled = true;
            }
        }
        private static string GetDestinationPath(string filename, string foldername)
        {
            string appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = string.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var selectedUser = usersRepository.Find(u => u.UserName == txtUsername.Text).SingleOrDefault();
            var selectedCompany = companiesRepository.Find(c => c.ID == selectedUser.CompanyId).SingleOrDefault();

            string destenationPathLogo = GetDestinationPath(selectedCompany.LogoImagePath, @"Static\Logos");
            BitmapImage bitmapLogo = new BitmapImage();
            bitmapLogo.BeginInit();
            bitmapLogo.UriSource = new Uri(destenationPathLogo);
            bitmapLogo.EndInit();
            imgLogo.Source = bitmapLogo;

            string destenationPathSignutre = GetDestinationPath(selectedCompany.SignutreImagePath, @"Static\Signutres");
            BitmapImage bitmapSignutre = new BitmapImage();
            bitmapSignutre.BeginInit();
            bitmapSignutre.UriSource = new Uri(destenationPathSignutre);
            bitmapSignutre.EndInit();
            imgSignutre.Source = bitmapSignutre;




            dgProducts.ItemsSource = OrderDetails;

        }
    }
}
