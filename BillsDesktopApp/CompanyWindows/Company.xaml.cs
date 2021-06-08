using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BillsDesktopApp.CompanyWindows
{
    /// <summary>
    /// Interaction logic for Company.xaml
    /// </summary>
    public partial class Company : Window
    {
        private readonly BillsContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Companies> CompaniesService;
        private Companies selectedCompany;
        public static ObservableCollection<Companies> CompaniesObservalbleCollection = new ObservableCollection<Companies>();

        public Company(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            CompaniesService = unitOfWork.Repository<Companies>();
            InitializeComponent();
        }

        private void BtnLogoBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                imgLogo.Source = bitmap;


                string name = Path.GetFileName(selectedFileName);
                string destenationPath = GetDestinationPath(name, @"Static\Logos");

                File.Copy(selectedFileName, destenationPath, true);

                selectedCompany.LogoImagePath = name;
                lblLogoFilePath.Content = name;
            }
        }

        private void BtnSignutreBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                imgSignutre.Source = bitmap;

                string name = Path.GetFileName(selectedFileName);
                string destenationPath = GetDestinationPath(name, @"Static\Signutres");

                File.Copy(selectedFileName, destenationPath, true);

                lblSignutreFilePath.Content = name;
                selectedCompany.SignutreImagePath = name;
            }
        }

        private static string GetDestinationPath(string filename, string foldername)
        {
            string appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = string.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            selectedCompany = CompaniesService.Find(c => c.TaxNumber == txtTaxNumber.Text &&
                c.Name.ToLower() == txtCompanyName.Text.ToLower()).SingleOrDefault();

            if (selectedCompany.LogoImagePath != null)
            {
                string destenationPath = GetDestinationPath(selectedCompany.LogoImagePath, @"Static\Logos");
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destenationPath);
                bitmap.EndInit();
                imgLogo.Source = bitmap;
                lblLogoFilePath.Content = selectedCompany.LogoImagePath;
            }
            else
            {
                string destenationPath = GetDestinationPath("logo-placeholder.jpeg", @"Static\Logos");
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destenationPath);
                bitmap.EndInit();
                imgLogo.Source = bitmap;
                lblLogoFilePath.Content = "logo-placeholder.jpeg";
            }

            if (selectedCompany.SignutreImagePath != null)
            {
                string destenationPath = GetDestinationPath(selectedCompany.SignutreImagePath, @"Static\Signutres");
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destenationPath);
                bitmap.EndInit();
                imgSignutre.Source = bitmap;
                lblSignutreFilePath.Content = selectedCompany.SignutreImagePath;
            }
            else
            {
                string destenationPath = GetDestinationPath("SignutrePlaceHolder.png", @"Static\Signutres");
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destenationPath);
                bitmap.EndInit();
                imgLogo.Source = bitmap;
                lblSignutreFilePath.Content = "SignutrePlaceHolder.png";
            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var result = CompaniesService.Update(selectedCompany);

            if (result < 1)
            {
                MessageBox.Show("خطأ", "حدث خطأ بتحديث صورة التوقيع الإلكتروني", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("تم", "تم تحديث صورة ", MessageBoxButton.OK, MessageBoxImage.Information); ;
            }
        }
    }
}
