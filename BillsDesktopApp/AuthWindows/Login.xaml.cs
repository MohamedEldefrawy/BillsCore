using BillsDesktopApp.DashboardWindow;
using DAL;
using DAL.Repositories.Origin;
using DAL.UnitOfWork;
using System.Windows;
using static Utilities.Utilities.Utilities;

namespace BillsDesktopApp.AuthWindows
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<BL.Models.Users> UsersService;


        public Login()
        {
            unitOfWork = new UnitOfWork(new ContextFactory().CreateDbContext(new string[1]));
            UsersService = unitOfWork.Repository<BL.Models.Users>();
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoadingSpinner.Visibility = Visibility.Visible;

            BL.Models.Users user = null;

            if (LoadingSpinner.Visibility == Visibility.Visible)
            {
                user = await UsersService.FindAsync(u => u.UserName.ToLower() == txtUserName.Text.ToLower());
            }


            if (user == null)
            {
                LoadingSpinner.Visibility = Visibility.Hidden;
                MessageBox.Show("قمت بإدخال اسم مستخدم خطأ", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                var isPwCorrect = UnHash(txtPassword.Password, user.Password);
                if (isPwCorrect)
                {
                    Dashboard dashboard = new Dashboard(new ContextFactory().CreateDbContext(new string[1]));
                    dashboard.lblUserId.Content = user.ID;
                    dashboard.lblWelcome.Content = " مرحباً " + user.UserName;
                    Application.Current.MainWindow = dashboard;
                    Close();
                    dashboard.Show();
                }
                else
                {
                    LoadingSpinner.Visibility = Visibility.Hidden;
                    MessageBox.Show("قمت بإدخال كلمة سر خطأ", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); ;
                }
            }
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            SignUp frmSignUp = new SignUp(new ContextFactory().CreateDbContext(new string[1]));
            frmSignUp.Show();
            this.Close();
        }
    }
}
