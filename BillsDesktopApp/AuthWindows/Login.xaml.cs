using BillsDesktopApp.DashboardWindow;
using DAL;
using DAL.UnitOfWork;
using System.Linq;
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


        public Login()
        {
            unitOfWork = new UnitOfWork(new ContextFactory().CreateDbContext(new string[1]));
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = unitOfWork.Repository<BL.Models.Users>().Find(u => u.UserName.ToLower() == txtUserName.Text.ToLower())
                .FirstOrDefault();

            if (user == null)
            {
                MessageBox.Show("خطأ", "قمت بإدخال اسم مستخدم خطأ", MessageBoxButton.OK, MessageBoxImage.Error); ;

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
                    MessageBox.Show("خطأ", "قمت بإدخال كلمة سر خطأ", MessageBoxButton.OK, MessageBoxImage.Error); ;
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
