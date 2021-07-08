using PrPract.Classes;
using PrPract.Database;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PrPract
{
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string Login = TBLogin.Text, Password = PBPassword.Password;

            if (!LogIn(Login, Password))
                return;

            OpenMainWindow();
        }

        private bool LogIn(string Login, string Password)
        {
            if (DataChecker.CheckFieldsAuth(ref Login, ref Password) == false)
            {
                IsEnabled = true;
                return false;
            }

            Пользователь User = FindUser(Login);

            if (DataChecker.CheckUser(User, ref Password) == false)
            {
                IsEnabled = true;
                return false;
            }

            Properties.Settings.Default.UserRole = User.Роль;

            return true;
        }

        private static Пользователь FindUser(string Login)
        {
            using (var dbcontext = new АСКУЭEntities())
                return dbcontext.Пользователь.FirstOrDefault(i => i.Логин == Login);
        }

        private void OpenMainWindow()
        {
            new MainWindow().Show();
            Close();
        }
    }
}