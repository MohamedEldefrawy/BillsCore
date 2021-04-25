using static Utilities.Utilities.Utilities;
using System.Linq;
using System.Windows;
using BillsDesktopApp.ErrorMessages;
using DAL;
using DAL.UnitOfWork;

namespace BillsDesktopApp.AuthWindows
{
    /// <summary>
    /// Interaction logic for ChangeProfile.xaml
    /// </summary>
    public partial class ChangeProfile : Window
    {
        private readonly BillsContext _context;

        private readonly UnitOfWork unitOfWork;
        private bool IsValid = false;

        public ChangeProfile(BillsContext Context)
        {
            _context = Context;
            unitOfWork = new UnitOfWork(_context);

            InitializeComponent();
            lblPasswordErrorMessage.Content = PasswordErrorMessages.Password;
            lblConsfirmPasswordErrorMessage.Content = PasswordErrorMessages.PasswordConfirm;
            lblNewPasswordErrorMessage.Content = PasswordErrorMessages.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = unitOfWork.Repository<BL.Models.Users>().Find(u => u.UserName.ToLower() == txtUserName.Text.ToLower()).FirstOrDefault();

            if (!IsValid)
            {
                MessageBox.Show("خطأ", "رجاء إخال بيانات صحيحة", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }
            else
            {
                if (user == null)
                {
                    MessageBox.Show("خطأ", "قمت بإدخال اسم مستخدم خطأ", MessageBoxButton.OK, MessageBoxImage.Error); ;

                }
                else
                {

                    var isPwCorrect = UnHash(txtPassword.Password, user.Password);
                    if (isPwCorrect)
                    {
                        user.Password = Hash(txtNewPassword.Password);
                        unitOfWork.Repository<BL.Models.Users>().Update(user);
                        int result = unitOfWork.Complete();

                        if (result < 0)
                        {
                            MessageBox.Show("خطأ", "حدث خطأ بتحديث كلمة السر", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                        else
                        {
                            MessageBox.Show("تم", "تم تحديث كلمة السر", MessageBoxButton.OK, MessageBoxImage.Information);

                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("خطأ", "قمت بإدخال كلمةالسر الحالية خطأ", MessageBoxButton.OK, MessageBoxImage.Error); ;
                    }

                }
            }

        }

        private void txtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtNewPassword.Password.Length < 3)
            {
                lblNewPasswordErrorMessage.Visibility = Visibility.Visible;
                IsValid = false;
            }

            else
            {
                lblNewPasswordErrorMessage.Visibility = Visibility.Hidden;
                IsValid = true;
            }
        }

        private void txtNewPasswordConfirmation_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtNewPassword.Password != txtNewPasswordConfirmation.Password)
            {
                lblConsfirmPasswordErrorMessage.Visibility = Visibility.Visible;
                IsValid = false;
            }
            else
            {
                lblConsfirmPasswordErrorMessage.Visibility = Visibility.Hidden;
                IsValid = true;
            }
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length < 3)
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
    }
}
