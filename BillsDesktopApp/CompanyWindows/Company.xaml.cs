using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using Microsoft.Win32;
using System;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            selectedCompany = CompaniesService.Find(c => c.TaxNumber == txtTaxNumber.Text &&
                c.Name.ToLower() == txtCompanyName.Text.ToLower()).SingleOrDefault();

            if (selectedCompany.LogoImagePath != null)
            {
                string destenationPath = Environment.CurrentDirectory + @"\Static\Logos\" + selectedCompany.LogoImagePath;
                BitmapImage bitmap = new();

                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destenationPath);
                bitmap.EndInit();
                imgLogo.Source = bitmap;
                lblLogoFilePath.Content = selectedCompany.LogoImagePath;


            }

            if (selectedCompany.SignutreImagePath != null)
            {
                string destenationPath = Environment.CurrentDirectory + @"\Static\Signutres\" + selectedCompany.SignutreImagePath;
                BitmapImage bitmap = new();

                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destenationPath);
                bitmap.EndInit();
                imgSignutre.Source = bitmap;
                lblSignutreFilePath.Content = selectedCompany.SignutreImagePath;

            }
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
                string destenationPath = Environment.CurrentDirectory + @"\Static\Logos\" + name;


                File.Copy(selectedFileName, destenationPath, true);
                File.SetAttributes(destenationPath, FileAttributes.Normal);



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
                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                imgSignutre.Source = bitmap;

                string name = Path.GetFileName(selectedFileName);
                string destenationPath = Environment.CurrentDirectory + @"\Static\Signutres\" + name;


                File.Copy(selectedFileName, destenationPath, true);
                File.SetAttributes(destenationPath, FileAttributes.Normal);




                lblSignutreFilePath.Content = name;
                selectedCompany.SignutreImagePath = name;
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
