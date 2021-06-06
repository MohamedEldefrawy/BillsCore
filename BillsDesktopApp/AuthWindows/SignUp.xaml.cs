using BillsDesktopApp.ErrorMessages;
using BL.Models;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static Utilities.Utilities.Utilities;

namespace BillsDesktopApp.AuthWindows
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private readonly BillsContext _context;
        private readonly IRepository<Users> UsersService;
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Companies> CompaniesService;
        private bool IsValid = false;
        public SignUp(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            UsersService = unitOfWork.Repository<Users>();
            CompaniesService = unitOfWork.Repository<Companies>();
            InitializeComponent();
            lblUserNameErrorMessage.Content = UserNameErrorMessages.UserNameMaxLength;
            lblConfirmPasswordErrorMessage.Content = PasswordErrorMessages.PasswordConfirm;
            lblPasswordErrorMessage.Content = PasswordErrorMessages.Password;
        }

        private void TxtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserName.Text.Length is > 15 or < 4)
            {
                lblUserNameErrorMessage.Visibility = Visibility.Visible;
                IsValid = false;
            }
            else
            {
                lblUserNameErrorMessage.Visibility = Visibility.Hidden;
                IsValid = true;

            }
        }

        private void TxtPasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPasswordConfirm.Password.ToString() != txtPassword.Password.ToString())
            {
                lblConfirmPasswordErrorMessage.Visibility = Visibility.Visible;
                IsValid = false;
            }
            else
            {
                lblConfirmPasswordErrorMessage.Visibility = Visibility.Hidden;
                IsValid = true;

            }
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.ToString().Length < 3)
            {
                lblPasswordErrorMessage.Visibility = Visibility.Visible;
                IsValid = false;
            }
            else
            {
                lblPasswordErrorMessage.Visibility = Visibility.Hidden;
                IsValid = true;
            }
        }

        private async void BtnSignup_Click(object sender, RoutedEventArgs e)
        {
            LoadingSpinner.Visibility = Visibility.Visible;

            var hashedPw = Hash(txtPassword.Password);
            var users = UsersService.GetAll();

            foreach (var user in users)

            {
                if (user.UserName.ToLower() == txtUserName.Text.ToLower())
                {
                    LoadingSpinner.Visibility = Visibility.Hidden;
                    MessageBox.Show("خطأ بالتسجيل", "إٍسم مستخدم موجود من قبل", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }

            var newuser = new Users
            {
                UserName = txtUserName.Text,
                Password = hashedPw
            };

            var company = new Companies
            {
                Name = txtCompanyName.Text,
                TaxNumber = txtTaxNumber.Text
            };

            if (!IsValid)
            {
                LoadingSpinner.Visibility = Visibility.Hidden;
                MessageBox.Show("خطأ بالتسجيل", "برجاء إدخال بيانات صالحة", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                if (LoadingSpinner.Visibility == Visibility.Visible)
                {
                    var addedCompany = CompaniesService.Add(company);

                    if (addedCompany > 0)
                    {
                        var selectedCompany = CompaniesService.Find(c => c.Name.ToLower() == txtCompanyName.Text &&
                        c.TaxNumber.ToLower() == txtTaxNumber.Text.ToLower()).SingleOrDefault();

                        newuser.CompanyId = selectedCompany.ID;
                    }


                    UsersService.AddAsync(newuser);
                }

            }

            int result = await unitOfWork.CompleteAsync();


            if (result < 1)
            {
                LoadingSpinner.Visibility = Visibility.Hidden;
                MessageBox.Show("حدث خطأ بالتسجيل", "خطأ بالتسجيل", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Login frmLogin = new Login();
                Application.Current.MainWindow = frmLogin;
                Close();
                frmLogin.Show();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Login frmLogin = new Login();
            frmLogin.Show();
            Application.Current.MainWindow = frmLogin;
            Close();
        }

        private void TxtCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtTaxNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
