using BillsDesktopApp.ErrorMessages;
using BL.Models;
using DAL;
using DAL.UnitOfWork;
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

        private readonly UnitOfWork unitOfWork;
        private bool IsValid = false;
        public SignUp(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);
            InitializeComponent();
            lblUserNameErrorMessage.Content = UserNameErrorMessages.UserNameMaxLength;
            lblConfirmPasswordErrorMessage.Content = PasswordErrorMessages.PasswordConfirm;
            lblPasswordErrorMessage.Content = PasswordErrorMessages.Password;
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserName.Text.Length > 15 || txtUserName.Text.Length < 4)
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

        private void txtPasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
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

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
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

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {

            var hashedPw = Hash(txtPassword.Password);


            if (!IsValid)
            {
                MessageBox.Show("خطأ بالتسجيل", "برجاء إدخال بيانات صالحة", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                unitOfWork.Repository<BL.Models.Users>().Add(new Users
                {
                    UserName = txtUserName.Text,
                    Password = hashedPw
                });
            }

            int result = unitOfWork.Complete();

            if (result < 1)
            {
                MessageBox.Show("حدث خطأ بالتسجيل", "خطأ بالتسجيل", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else
            {
                Login frmLogin = new Login(_context);
                Application.Current.MainWindow = frmLogin;
                Close();
                frmLogin.Show();
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Login frmLogin = new Login(_context);
            frmLogin.Show();
            Application.Current.MainWindow = frmLogin;
            this.Close();
        }
    }
}
