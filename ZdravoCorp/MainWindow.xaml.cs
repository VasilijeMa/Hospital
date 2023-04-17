using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
//using System.Collections;
using System.IO;
using Newtonsoft.Json;

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
            users = User.LoadAll();
            singleton = Singleton.Instance;

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
