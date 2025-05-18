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
using App.Abstraction;
using App.Services;
using Domain.Entities;

namespace HRM
{
    public partial class LoginWindow : Window
    {
        
        private IAuthenticationService authService;

        public LoginWindow()
        {
            InitializeComponent();
            var managerRepo = new ManagerHrRepositoryAdapter();
            authService = new ProxyAuthenticationService(managerRepo);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (authService.Login(username, password))
            {
                var currentUser = authService.GetCurrentUser(); // obții userul

                // Transmiți userul mai departe
                MainWindow main = new MainWindow(currentUser);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login failed!");
            }
        }
    }
}
