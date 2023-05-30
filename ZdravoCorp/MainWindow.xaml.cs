using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;

//using System.Collections;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> users;
        private UserService userService;
        private MedicamentsToAddService medicamentsToAddService = new MedicamentsToAddService();
        public MainWindow()
        {
            InitializeComponent();
            userService = new UserService();
            users = userService.GetUsers();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var user in users)
            {
                if (tbUsername.Text == user.Username && pbPassword.Password == user.Password)
                {
                    this.Visibility = Visibility.Hidden;
                    medicamentsToAddService.checkOrderedMedicaments();
                    userService.DisplayWindow(user);
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
