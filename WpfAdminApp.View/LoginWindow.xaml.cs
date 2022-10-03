using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAdminApp.ViewModel;
using WpfAdminApp.Model;

namespace WpfAdminApp.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserCollection allUsers;
        private string checkUsername;
        private string checkPassword;

        public UserCollection AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; }
        }

        public LoginWindow()
        {
            AllUsers = UserCollection.GetAllPersons();
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(UserFound())
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                loginMessage.Content = "Login failed, check your input";
            }
        }

        private bool UserFound()
        {
            checkUsername = checkForUsername.Text;
            checkPassword = checkForPassword.Text;
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (allUsers[i].UserName == checkUsername && allUsers[i].Password == checkPassword)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
