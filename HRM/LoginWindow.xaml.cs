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
using Infrastructure.Repositories;

namespace HRM
{
    public partial class LoginWindow : Window
    {
        
        private IAuthenticationService authService;

        public LoginWindow()
        {
            InitializeComponent();
            var managerRepo = new ManagerHrRepositoryAdapter();
            var proxy = new ProxyAuthenticationService(managerRepo);
            authService = new LoggingAuthenticationServiceDecorator(proxy);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (authService.Login(username, password))
            {
                var currentUser = authService.GetCurrentUser();

                MainWindow main = new MainWindow(currentUser);
                main.Show();
                this.Close();
            }
            else
            {
                // Aici poți verifica dacă motivul e "concediu"
                var managerRepo = new ManagerHrRepository();
                var employee = managerRepo.LoadAll().FirstOrDefault(e => e.Name == username && e.Role == "Manager");
                if (employee != null && employee.StateName == "În concediu")
                {
                    MessageBox.Show("Nu te poți loga: ești în concediu!");
                }
                else
                {
                    MessageBox.Show("Login failed!");
                }
            }
        }
    }
}
