using System.Collections.Generic;
using System.Windows;
//using System.Collections;
using ZdravoCorp.Domain;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> users;
        Singleton singleton;
        public MainWindow()
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            users = singleton.UserRepository.Users;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var user in users)
            {
                if (tbUsername.Text == user.Username && pbPassword.Password == user.Password)
                {
                    this.Visibility = Visibility.Hidden;
                    User.DisplayWindow(user);
                    this.Visibility = Visibility.Visible;
                    tbUsername.Text = "";
                    pbPassword.Password = "";
                    return;
                }
            }
            pbPassword.Password = "";
            MessageBox.Show("Invalid username or password.");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
