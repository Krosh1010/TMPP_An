using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Abstraction;
using Domain.Entities;

namespace App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private List<User> users;
        private User currentUser;

        public AuthenticationService()
        {
            // Exemplu hardcodat
            users = new List<User>
        {
            new User { Username = "hr", Password = "123", Role = "HR" },
            new User { Username = "managerB", Password = "456", Role = "Manager", Team = "Back" },
            new User { Username = "managerF", Password = "456", Role = "Manager", Team = "Front" },
            new User { Username = "managerT", Password = "456", Role = "Manager", Team = "Tester" }
        };
        }

        public bool Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                currentUser = user;
                return true;
            }
            return false;
        }

        public User GetCurrentUser()
        {
            return currentUser;
        }
    }
}